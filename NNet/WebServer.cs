using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NNet
{
    public class WebServer
    {
        public object owner;
        class HttpParams
        {
            public string name;
            public string value;

            public HttpParams(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
            static public List<HttpParams> GetFromURL(string url)
            {
                List<HttpParams> p = new List<HttpParams>();
                string a = url;
                if (a.IndexOf("?")<=0) return(p);
                url = a.Substring(0, a.IndexOf("?"));
                string parameters = a.Substring(a.IndexOf("?") + 1, a.Length - 1 - a.IndexOf("?"));
                string[] par = parameters.Split('&');
                for (int i = 0; i < par.Length; i++)
                {
                    string[] parame = par[i].Split('=');
                    p.Add(new HttpParams(parame[0], parame[1]));
                }
                return (p);
            }
            static public int GetIndexByName(List<HttpParams> url_params, string name)
            {
                for (int i = 0; i < url_params.Count; i++)
                {
                    if (url_params[i].name == name) return (i);
                }
                return (-1);
            }
            static public string GetValueByName(List<HttpParams> url_params, string name)
            {
                for (int i = 0; i < url_params.Count; i++)
                {
                    if (url_params[i].name == name) return (url_params[i].value);
                }
                return (null);
            }
            static public string GetValueByName(HttpListenerRequest req, string name)
            {
                for (int i = 0; i < req.Headers.Count; i++)
                {
                    if (req.Headers.GetKey(i) == name) return (req.Headers.Get(i));
                }
                return (null);
            }
            static public string GetNodeURL(string url)
            {
                if (url.IndexOf("?") <= 0) return (url);
                return(url.Substring(0, url.IndexOf("?")));
            }
            static public bool IsExistByName(HttpListenerResponse response, List<HttpParams> url_params, string name)
            {
                if (HttpParams.GetValueByName(url_params, name) != null) return (true);
                response.StatusCode = 422;
                return (false);
            }
            static public bool IsExistByName(HttpListenerResponse response, HttpListenerRequest req, string name)
            {
                if (HttpParams.GetValueByName(req, name) != null) return (true);
                response.StatusCode = 422;
                return (false);
            }
        }

        public async void Start()
        {
            await Listen();
        }
        private async Task Listen()
        {
            HttpListener listener = new HttpListener();

            listener.Prefixes.Add("http://localhost/");
            listener.Prefixes.Add("http://localhost/model/");
            listener.Prefixes.Add("http://localhost/model/dataset/");
            listener.Prefixes.Add("http://localhost/models/");
            listener.Prefixes.Add("http://localhost/models/types/");
            listener.Prefixes.Add("http://localhost/test/");

            listener.Start();

            while (true)
            {
                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    IPEndPoint point = request.RemoteEndPoint;  // IP адрес и номер порта клиента

                    // Формируем ответ сервера
                    string res = GetResponse(request, response);
                    if(res==null) res="";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(res);
                    response.ContentLength64 = buffer.Length;

                    if (response.StatusCode >= 400)
                        response.ContentType = "text/plain; charset=UTF-8";
                    else
                    {
                        if (response.ContentLength64 > 0)
                        {
                            response.ContentType = "application/json; charset=UTF-8";
                            response.AddHeader("Accept", "application/json, */*; q=0.01");
                        }
                    }

                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        public string GetResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "OPTIONS":
                case "HEAD":
                case "GET":
                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    switch (HttpParams.GetNodeURL(request.RawUrl))
                    {
                        case "/model/":
                        case "/model":
                            return (Api_model(request, response));

                        case "/model/dataset/":
                        case "/model/dataset":
                            return (Api_dataset(request, response));

                        case "/models/":
                        case "/models":
                            return (Api_models(request, response));

                        case "/models/types/":
                        case "/models/types":
                            return (Api_neuro_types(request, response));

                        default:
                            response.StatusCode = 418;
                            return ("");
                    }
                default:
                    response.StatusCode = 501;
                    return ("");
            }
        }

        public string Api_dataset(HttpListenerRequest request, HttpListenerResponse response)
        {
            bool is_out = false;
            int id = -1;
            string name = null;
            Form1 form = (Form1)owner;
            EventArgs e = new EventArgs();
            StreamReader Reader = new StreamReader(request.InputStream, request.ContentEncoding);

            // Предобработка ответа
            switch (request.HttpMethod)
            {
                case "OPTIONS":
                    response.AddHeader("Allow", "HEAD,GET,POST,DELETE");
                    return ("");

                case "HEAD":
                case "GET":
                    List<HttpParams> url_params = HttpParams.GetFromURL(request.RawUrl);
                    if (!HttpParams.IsExistByName(response, url_params, "uuid"))
                        return ("Error: required url parameter 'uuid' not found!");

                    id = Classifire.GetIndexByUuid(form.classifirs, HttpParams.GetValueByName(url_params, "uuid"));
                    break;

                case "POST":
                case "DELETE":
                    if (!HttpParams.IsExistByName(response, request, "uuid"))
                        return ("Error: required header parameter 'uuid' not found!");

                    is_out = HttpParams.IsExistByName(response, request, "output");

                    id = Classifire.GetIndexByUuid(form.classifirs, HttpParams.GetValueByName(request, "uuid"));
                    name = HttpParams.GetValueByName(request, "name");
                    break;

                default:
                    response.StatusCode = 501;
                    return ("");
            }

            if (id < 0)
            {
                response.StatusCode = 404;
                return ("Error: uuid not found!");
            }

            // Формирование ответа
            switch (request.HttpMethod)
            {
                case "HEAD":
                    return ("");

                case "GET":
                    return (JsonConvert.SerializeObject(form.classifirs[id].GetDataSet())); // Найти по имени и в выводах или входах

                case "POST":
                    try
                    {
                        if (is_out)
                        {
                            if (name == null) name = "noname";
                            name = "out: " + name;
                            form.classifirs[id].data.outputs.Add(new Classifire.DataSet.Parameter(new List<double>(JsonConvert.DeserializeObject<List<double>>(Reader.ReadToEnd()))));
                            form.classifirs[id].data.outputs[form.classifirs[id].data.outputs.Count - 1].name = name;
                        }
                        else
                        {
                            if (name == null) name = "noname";
                            name = "in: " + name;
                            form.classifirs[id].data.inputs.Add(new Classifire.DataSet.Parameter(new List<double>(JsonConvert.DeserializeObject<List<double>>(Reader.ReadToEnd()))));
                            form.classifirs[id].data.inputs[form.classifirs[id].data.inputs.Count - 1].name = name;
                        }

                        response.AddHeader("Location", "/model/dataset/" + (is_out ? "outputs/" : "inputs/") + name);
                        response.StatusCode = 201; // Created

                        if (id == form.selected)
                        {
                            form.DataSetToDataGrid();
                            form.ColumnTypesSet();
                        }
                    }
                    catch
                    {
                        response.StatusCode = 415; //No falid data (json)
                        return ("Error: not valid JSON");
                    }
                    return("");

                case "DELETE":
                    bool is_delete = false;
                    if (name == null)
                    {
                        form.classifirs[id].data.Clear();
                        is_delete = true;
                    }
                    else
                    {
                        if (is_out)
                        {
                            for (int i = 0; i < form.classifirs[id].data.outputs.Count; i++)
                            {
                                if (form.classifirs[id].data.outputs[i].name != name) continue;
                                form.classifirs[id].data.outputs.RemoveAt(i);
                                is_delete = true;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < form.classifirs[id].data.inputs.Count; i++)
                            {
                                if (form.classifirs[id].data.inputs[i].name != name) continue;
                                form.classifirs[id].data.inputs.RemoveAt(i);
                                is_delete = true;
                            }
                        }
                    }
                    if(is_delete) response.StatusCode = 204;
                    else response.StatusCode = 404;
                    return ("");

                default:
                    return ("");
            }
        }
        public class ApiUuidName
        {
            public string uuid;
            public string name;
            public ApiUuidName() { }
            public ApiUuidName(string uuid, string name)
            {
                this.uuid = uuid;
                this.name = name;
            }
        }
        public string Api_neuro_types(HttpListenerRequest request, HttpListenerResponse response)
        {
            Form1 form = (Form1)owner;

            switch (request.HttpMethod)
            {
                case "OPTIONS":
                    response.AddHeader("Allow", "HEAD,GET");
                    return ("");

                case "HEAD":
                    return ("");

                case "GET":
                    List<ApiUuidName> types = new List<ApiUuidName>();
                    for (int i = 0; i < Classifire.type_names.Length; i++)
                    {
                        types.Add(new ApiUuidName(i.ToString(), Classifire.type_names[i]));
                    }
                    return (JsonConvert.SerializeObject(types));

                default:
                    response.StatusCode = 501;
                    return ("");
            }
        }
        public string Api_models(HttpListenerRequest request, HttpListenerResponse response)
        {
            Form1 form = (Form1)owner;

            switch (request.HttpMethod)
            {
                case "OPTIONS":
                    response.AddHeader("Allow", "HEAD,GET");
                    return ("");

                case "HEAD":
                    return("");

                case "GET":
                    List<ApiUuidName> models = new List<ApiUuidName>();
                    for (int i = 0; i < form.classifirs.Count; i++)
                    {
                        models.Add(new ApiUuidName(form.classifirs[i].uuid, form.classifirs[i].name));
                    }
                    return (JsonConvert.SerializeObject(models));

                default:
                    response.StatusCode = 501;
                    return ("");
            }
        }
        public string Api_model(HttpListenerRequest request, HttpListenerResponse response)
        {
            int id = -1, type = -1;
            Form1 form = (Form1)owner;
            EventArgs e = new EventArgs();
            StreamReader Reader = new StreamReader(request.InputStream, request.ContentEncoding);

            // Предобработка ответа
            switch (request.HttpMethod)
            {
                case "OPTIONS":
                    response.AddHeader("Allow", "HEAD,GET,POST,PUT,PATCH,DELETE");
                    return ("");

                case "HEAD":
                case "GET":
                    List<HttpParams> url_params = HttpParams.GetFromURL(request.RawUrl);
                    if (!HttpParams.IsExistByName(response, url_params, "uuid"))
                        return ("Error: required url parameter 'uuid' not found!");

                    id = Classifire.GetIndexByUuid(form.classifirs, HttpParams.GetValueByName(url_params, "uuid"));
                    if (id >= 0) break;

                    response.StatusCode = 404;
                    return ("Error: uuid not found!");

                case "POST":
                case "PATCH":
                case "PUT":
                case "DELETE":
                    if (!HttpParams.IsExistByName(response, request, "uuid"))
                        return ("Error: required header parameter 'uuid' not found!");

                    if (request.HttpMethod != "DELETE")
                    {
                        if (!HttpParams.IsExistByName(response, request, "type"))
                            return ("Error: required header parameter 'type' not found!");
                        type = Convert.ToInt32(HttpParams.GetValueByName(request, "type"));
                    }

                    if (request.HttpMethod != "POST")
                    {
                        id = Classifire.GetIndexByUuid(form.classifirs, HttpParams.GetValueByName(request, "uuid"));
                        if (id >= 0) break;

                        response.StatusCode = 404;
                        return ("Error: uuid not found!");
                    }
                    else
                    {
                        form.button_model_add_new_Click(owner, e);
                        id = form.classifirs.Count - 1;
                        break;
                    }

                default:
                    response.StatusCode = 501;
                    return ("");
            }

            // Формирование ответа
            switch (request.HttpMethod)
            {
                case "HEAD": 
                    return ("");
                
                case "GET": 
                    return (JsonConvert.SerializeObject(form.classifirs[id]));

                case "POST":
                case "PATCH":
                case "PUT":
                    try
                    {
                        switch ((ENUM_CLASSIFIRIES)type)
                        {
                            case ENUM_CLASSIFIRIES.NN:
                            case ENUM_CLASSIFIRIES.RNN:
                                form.classifirs[id] = JsonConvert.DeserializeObject<NeuroNet>(Reader.ReadToEnd());
                                break;
                            case ENUM_CLASSIFIRIES.CNN:
                                break;
                            case ENUM_CLASSIFIRIES.LSTM:
                                break;
                            default:
                                response.StatusCode = 404;
                                return ("Error: type does not exist!");
                        }
                    }
                    catch
                    {
                        response.StatusCode = 415; //No falid data (json)
                        return ("Error: not valid JSON");
                    }

                    if (request.HttpMethod == "POST")
                    {
                        response.AddHeader("Location", "/model/" + form.classifirs[id].uuid);
                        response.StatusCode = 201; // Created
                        return ("");
                    }

                    if (form.selected == id) form.UpdateInterface(); // Обновляем интерфейс

                    if (request.HttpMethod == "PUT")
                    {
                        response.StatusCode = 204; // No data
                        return ("");
                    }

                    return (JsonConvert.SerializeObject(form.classifirs[id]));

                case "DELETE":
                    form.button_model_remove_Click(id, e);
                    response.StatusCode = 204;
                    return ("");

                default:
                    return ("");
            }
        }
    }
}

