using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Text;

namespace BlogProjectAPI.Middlewares
{
    public class AccessLogger
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public AccessLogger(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        private AccessLogs accessLogs;
        public async Task Invoke(HttpContext context, IAccessLoggerRepository accessLogerRepository)
        {
            accessLogs = new AccessLogs()
            {
                Host = context.Request.Host.ToString(),
                Path = context.Request.Path,
                Scheme = context.Request.Scheme,
                QueryString = string.IsNullOrEmpty(context.Request.QueryString.Value) ? null : context.Request.QueryString.Value,
                WhoRequested = string.IsNullOrEmpty(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ? null : context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Time = DateTime.Now
            };
            await LogRequest(context);
            await LogResponse(context);
            await accessLogerRepository.LogThisAccess(accessLogs);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            accessLogs.RequestBody = ReadStreamInChunks(requestStream);

            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            accessLogs.ResponseBody = text;
            accessLogs.StatusCode = context.Response.StatusCode;

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }

}

