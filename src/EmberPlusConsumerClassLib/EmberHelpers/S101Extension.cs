﻿#region copyright
/*
 * NuGet EmBER+ Consumer Lib
 *
 * Copyright (c) 2023 Roger Sandholm & Fredrik Bergholtz, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion copyright

using Lawo.EmberPlusSharp.Glow;
using Lawo.EmberPlusSharp.S101;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EmberPlusConsumerClassLib.EmberHelpers
{
    public static class S101Extension
    {
        private static readonly int S101ClientTimeout = 6000;
        private static readonly int S101ClientBufferSize = 8192;

        /// <summary>
        /// Helps to create a <see cref="TcpClient"/> and an <see cref="S101Client"/> used for consumer connection.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static async Task<S101Client> CreateClient(string host, int port, ILogger logger)
        {
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port);
            NetworkStream stream = tcpClient.GetStream();

            S101Logger s101Logger = logger.IsEnabled(LogLevel.Debug) ? new S101Logger(GlowTypes.Instance, new ILoggerTraceWriter(logger)) : null;

            S101Client client = new S101Client(
                tcpClient,
                stream.ReadAsync,
                stream.WriteAsync,
                s101Logger,
                S101ClientTimeout,
                S101ClientBufferSize);

            return client;
        }
    }
}
