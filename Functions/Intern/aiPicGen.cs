using System.Net;
using System.Net.WebSockets;
using System.Text;

using ByteWizardApi.Model.Intern;
using ByteWizardApi.Model.Intern.Checkpoints;

using Newtonsoft.Json;

namespace ByteWizardApi.Functions.Intern
{
    /// <summary>
    /// The <c>AiPicGen</c> class provides functionalities to generate images and tokens asynchronously.
    /// </summary>
    /// <remarks>
    /// This class is designed to work within the internal namespace and is primarily used for image and token generation using asynchronous operations.
    /// </remarks>
    internal sealed class AiPicGen()
    {
        /// <summary>
        /// WebSocket URL for establishing a connection to the ComfyUI server.
        /// </summary>
        private const string server = "ws://**********/";


        /// <summary>
        /// The IP address used by the <c>AiPicGen</c> class for network operations.
        /// </summary>
        private const string ip = "************";

        /// <summary>
        /// Constant representing the hostname for Windows
        /// </summary>
        private const string HostnameWindows = "********";


        /// <summary>
        /// Asynchronously generates images based on the provided generation data.
        /// </summary>
        /// <param name="data">The data used for generating images, including prompts and user settings.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of byte arrays representing the generated images.
        /// Returns null if the image generation fails.
        /// </returns>
        /// <remarks>
        /// This method reads a JSON configuration file, updates the client ID, Lora model settings, prompts,
        /// and seed value, then connects to a WebSocket server to generate images using the specified generation data.
        /// </remarks>
        public async Task<List<byte[]>?> GeneratePicAsync(GeneratData data)
        {
            IPHostEntry hostEntry = await Dns.GetHostEntryAsync(ip);
            
            string path = Path.Combine("Model", "Intern", "Checkpoints", "command.json");
            dynamic? aiWorkflow = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(path));
            List<byte[]>? images = null;
            
            aiWorkflow!["client_id"] = Guid.NewGuid().ToString();
            
            aiWorkflow["prompt"]["3"]["inputs"]["lora_count"] = 5;

            string Lora1;
            string Lora2;
            string Lora3;
            string Lora4;
            string Lora5;

            if(hostEntry.HostName.StartsWith(HostnameWindows))
            {
                Lora1 = data.UserSettings.Lora1.GetDescriptionWin()!;
                Lora2 = data.UserSettings.Lora2.GetDescriptionWin()!;
                Lora3 = data.UserSettings.Lora3.GetDescriptionWin()!;
                Lora4 = data.UserSettings.Lora4.GetDescriptionWin()!;
                Lora5 = data.UserSettings.Lora5.GetDescriptionWin()!;
            }
            else
            {
                Lora1 = data.UserSettings.Lora1.GetDescriptionLinux()!;
                Lora2 = data.UserSettings.Lora2.GetDescriptionLinux()!;
                Lora3 = data.UserSettings.Lora3.GetDescriptionLinux()!;
                Lora4 = data.UserSettings.Lora4.GetDescriptionLinux()!;
                Lora5 = data.UserSettings.Lora5.GetDescriptionLinux()!;
            }


            aiWorkflow["prompt"]["3"]["inputs"]["lora_name_1"] = Lora1;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_name_2"] = Lora2;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_name_3"] = Lora3;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_name_4"] = Lora4;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_name_5"] = Lora5;
            
            aiWorkflow["prompt"]["3"]["inputs"]["lora_wt_1"] = data.UserSettings.Strength1;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_wt_2"] = data.UserSettings.Strength2;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_wt_3"] = data.UserSettings.Strength3;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_wt_4"] = data.UserSettings.Strength4;
            aiWorkflow["prompt"]["3"]["inputs"]["lora_wt_5"] = data.UserSettings.Strength5;
            
            if(hostEntry.HostName.StartsWith(HostnameWindows))
            {
                aiWorkflow["prompt"]["5"]["inputs"]["ckpt_name"] = data.UserSettings.Model.GetDescriptionWin()!;
            }
            else
            {
                aiWorkflow["prompt"]["5"]["inputs"]["ckpt_name"] = data.UserSettings.Model.GetDescriptionLinux()!;
            }
            
            aiWorkflow["prompt"]["6"]["inputs"]["text"] = data.PositivePrompt;
            
            aiWorkflow["prompt"]["7"]["inputs"]["text"] = data.NegativePrompt;
            
            int seedValue = new Random().Next();
            aiWorkflow.prompt["9"]["inputs"]["seed"] = seedValue;
            
            ClientWebSocket client = new();

            await client.ConnectAsync(new Uri(server + "ws?clientId=" + aiWorkflow["client_id"]), CancellationToken.None);

            if(client.State == WebSocketState.Open)
            {
                images = await GenerateImage(client, aiWorkflow);
                return images;
            }
            else
            {
                return images;
            }
        }

        /// <summary>
        /// Generates token images based on the provided token data.
        /// </summary>
        /// <param name="data">The token data used for image generation, which includes prompts and other parameters.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of byte arrays representing the generated images. 
        /// Returns null if the image generation fails.
        /// </returns>
        /// <remarks>
        /// This method reads a JSON configuration file, updates the client ID and prompt information, 
        /// and then connects to a WebSocket server to generate images based on the token data.
        /// </remarks>
        public async Task<List<byte[]>?> TokenGenerator(TokenData data)
        {
            string path = Path.Combine("Model", "Intern", "Checkpoints", "tokenModel.json");
            dynamic? aiWorkflow = JsonConvert.DeserializeObject<dynamic>(await File.ReadAllTextAsync(path));

            List<byte[]>? images = null;
            
            aiWorkflow!["client_id"] = Guid.NewGuid().ToString();

            aiWorkflow["prompt"]["4"]["inputs"]["Text"] = data.PositivePrompt;
            
            ClientWebSocket client = new();

            await client.ConnectAsync(new Uri(server + "ws?clientId=" + aiWorkflow["client_id"]), CancellationToken.None);

            if(client.State == WebSocketState.Open)
            {
                images = await GenerateImage(client, aiWorkflow);
                return images;
            }
            else
            {
                return images;
            }
        }


        /// <summary>
        /// Sends a serialized AI workflow to a specified endpoint to generate images.
        /// </summary>
        /// <param name="ws">The client web socket used for communication with external services.</param>
        /// <param name="aiWorkflow">The dynamic AI workflow data that contains information for image generation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of byte arrays representing the generated images.
        /// </returns>
        /// <remarks>
        /// This method serializes the AI workflow data into JSON format, sends it to a specified HTTP endpoint, and processes the response to extract generated images.
        /// It includes error handling to manage failed requests and log exceptions.
        /// </remarks>
        private static async Task<List<byte[]>> GenerateImage(ClientWebSocket ws, dynamic aiWorkflow)
        {
            string prompt = JsonConvert.SerializeObject(aiWorkflow);
            StringContent data = new(prompt, Encoding.UTF8, "application/json");
            HttpClient client = new();

            List<byte[]> images = [];

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://***********:8188/prompt", data);

                if(response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString)!;
                    byte[] buffer = new byte[4096];

                    while(true)
                    {
                        WebSocketReceiveResult result;
                        ArraySegment<byte> message = new(buffer);
                        result = await ws.ReceiveAsync(message, CancellationToken.None);

                        if(result.MessageType == WebSocketMessageType.Text)
                        {
                            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            dynamic? messageObject = JsonConvert.DeserializeObject<dynamic>(receivedMessage);

                            if(messageObject!["type"] == "executing" && messageObject["data"]["node"] == null && messageObject["data"]["prompt_id"] == responseObject["prompt_id"])
                            {
                                break; // Execution is done
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    string id = responseObject["prompt_id"];

                    dynamic? history = await GetHistoryAsync(id);

                    foreach(dynamic? output in history[id]["outputs"])
                    {
                        foreach(dynamic image in output.Value["images"])
                        {
                            images.Add(GetImages(image));
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return images;
        }


        /// <summary>
        /// Retrieves the image data from a specified URL based on the provided image metadata.
        /// </summary>
        /// <param name="image">Dynamic object containing metadata such as filename, subfolder, and type used to locate the image.</param>
        /// <returns>
        /// A byte array representing the content of the image retrieved from the server.
        /// Returns an empty array if the retrieval process fails.
        /// </returns>
        /// <remarks>
        /// This method constructs a URL using the image metadata, sends an HTTP GET request to the server,
        /// and returns the image data in byte array format. It relies on synchronous HTTP requests,
        /// which may block if the server response is delayed.
        /// </remarks>
        private static byte[] GetImages(dynamic image)
        {
            HttpClient client = new();
            Uri Url = new($@"http://*********:8188/view?filename={image["filename"]}&subfolder={image["subfolder"]}&type={image["type"]}");
            HttpResponseMessage response = client.GetAsync(Url).Result;

            return response.Content.ReadAsByteArrayAsync().Result;
        }

        /// <summary>
        /// Asynchronously retrieves the generation history for a given prompt ID.
        /// </summary>
        /// <param name="prompt_id">The unique identifier for the prompt whose history is to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a dynamic object
        /// representing the history of the specified prompt.
        /// </returns>
        /// <remarks>
        /// This method sends an HTTP GET request to the specified server endpoint to obtain the history information
        /// for the provided prompt ID, and deserializes the response into a dynamic object.
        /// </remarks>
        private static async Task<dynamic> GetHistoryAsync(string prompt_id)
        {
            HttpClient client = new();
            HttpResponseMessage resposen = await client.GetAsync(new Uri("http://*******:8188/history/" + prompt_id));
            dynamic? result = JsonConvert.DeserializeObject<dynamic>(await resposen.Content.ReadAsStringAsync());
            return result!;
        }
    }
}