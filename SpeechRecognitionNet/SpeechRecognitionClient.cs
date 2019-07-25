using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechRecognitionNet
{
    public class SpeechRecognitionClient : IDisposable
    {
        private ClientWebSocket _client;

        private const int BufferSize = 1024;

        public event EventHandler<SpeechRecognitionEventArgs> Recognize;
        public event EventHandler<EventArgs> Close;

        public SpeechRecognitionClient()
        {
            _client = new ClientWebSocket();
        }

        public bool IsConnect() => _client?.State == WebSocketState.Open;

        public void Connect(string address, int port)
        {
            Task.Run(async () =>
                {
                    await _client.ConnectAsync(new Uri($"ws://{address}:{port}/ws"), CancellationToken.None);
                    var send = Encoding.UTF8.GetBytes("console app");
                    await _client.SendAsync(new ArraySegment<byte>(send), WebSocketMessageType.Text, true,
                        CancellationToken.None);
                    var buff = new ArraySegment<byte>(new byte[BufferSize]);
                    while (_client.State == WebSocketState.Open)
                    {
                        try
                        {
                            var result = await _client.ReceiveAsync(buff, CancellationToken.None);
                            var data = buff.Take(result.Count).ToArray();
                            var recognized = Encoding.UTF8.GetString(data);
                            Recognize?.Invoke(this, new SpeechRecognitionEventArgs(recognized));
                        }
                        catch
                        {
                            break;
                        }
                    }

                    Disconnect();
                });
        }

        public void Disconnect()
        {
            if (IsConnect())
            {
                _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                Close?.Invoke(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            _client.Dispose();
            _client = null;
        }
    }
}
