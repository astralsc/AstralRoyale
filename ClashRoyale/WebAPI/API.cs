using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ClashRoyale;

namespace ClashRoyale.WebAPI
{
    public static class API
    {
        private static int Port = GetPortFromConfig();
        private static HttpListener Listener;
        private static Thread WebAPIThread;

        public static void Start()
        {
            WebAPIThread = new Thread(StartSafe);
            WebAPIThread.IsBackground = true;
            WebAPIThread.Start();
        }

        private static void StartSafe()
        {
            try
            {
                Logger.Log($"Attempting to start WebAPI on port '{Port}'...", null);

                if (!HttpListener.IsSupported)
                {
                    Logger.Log("The current system doesn't support the WebAPI.", null);
                    return;
                }

                Listener = new HttpListener();

                // Only bind to localhost and 127.0.0.1
                Listener.Prefixes.Add($"http://localhost:{Port}/");
                Listener.Prefixes.Add($"http://127.0.0.1:{Port}/");

                Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

                try
                {
                    Listener.Start();
                }
                catch (HttpListenerException hex)
                {
                    Logger.Log($"Failed to start WebAPI: {hex.Message} (Error Code: {hex.ErrorCode}). Port may be in use.", null);
                    return;
                }

                Logger.Log($"The WebAPI has been started on port '{Port}'.", null);

                while (Listener != null && Listener.IsListening)
                {
                    try
                    {
                        var context = Listener.GetContext();
                        ThreadPool.QueueUserWorkItem(c => HandleRequestSafe((HttpListenerContext)c), context);
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (HttpListenerException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"WebAPI encountered an unexpected error and will stop: {ex.Message}", null);
                        Stop();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"WebAPI failed to start: {ex.Message}", null);
            }
        }

        public static void Stop()
        {
            try
            {
                if (Listener != null)
                {
                    if (Listener.IsListening)
                        Listener.Stop();

                    Listener.Close();
                    Listener = null;
                }

                Logger.Log("The WebAPI has been stopped.", null);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error stopping the WebAPI: {ex.Message}", null);
            }
        }

        private static void HandleRequestSafe(HttpListenerContext context)
        {
            try
            {
                HandleRequest(context);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error processing WebAPI request: {ex.Message}", null);
            }
        }

        private static void HandleRequest(HttpListenerContext context)
        {
            string responseText;
            try
            {
                if (context.Request.Url.ToString().EndsWith("api/"))
                    responseText = GetjsonAPI();
                else
                    responseText = GetStatisticHTML();

                byte[] responseBuf = Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = responseBuf.Length;
                context.Response.OutputStream.Write(responseBuf, 0, responseBuf.Length);
                context.Response.OutputStream.Close();
            }
            catch
            {
                try
                {
                    context.Response.StatusCode = 500;
                    byte[] errorBuf = Encoding.UTF8.GetBytes("Internal Server Error");
                    context.Response.ContentLength64 = errorBuf.Length;
                    context.Response.OutputStream.Write(errorBuf, 0, errorBuf.Length);
                    context.Response.OutputStream.Close();
                }
                catch { }
            }
        }

        private static int GetPortFromConfig()
        {
            return 8888; // fixed port
        }

        public static string GetStatisticHTML()
        {
            try
            {
                return HTML()
                    .Replace("%ONLINEPLAYERS%", Resources.Players.Count.ToString())
                    .Replace("%INMEMORYPLAYERS%", API_Stats.PlayerStat.ToString())
                    .Replace("%INMEMORYALLIANCES%", API_Stats.AllianceStat.ToString());
            }
            catch (Exception ex)
            {
                Logger.Log($"Error generating statistics HTML: {ex.Message}", null);
                return "The server encountered an internal error or misconfiguration. (500)";
            }
        }

        public static string GetjsonAPI()
        {
            try
            {
                JObject data = new JObject
                {
                    { "online_players", Resources.Players.Count },
                    { "in_mem_players", API_Stats.PlayerStat },
                    { "in_mem_alliances", API_Stats.AllianceStat }
                };
                return JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error generating JSON API: {ex.Message}", null);
                return JsonConvert.SerializeObject(new { error = "Internal Server Error" }, Formatting.Indented);
            }
        }

        public static string HTML()
        {
            try
            {
                using (StreamReader sr = new StreamReader("WebAPI/HTML/Statistics.html"))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return "HTML file not found or could not be loaded.";
            }
        }
    }
}