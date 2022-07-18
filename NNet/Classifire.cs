using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace NNet
{
    public enum ENUM_CLASSIFIRIES
    {
        NULL=-1,
        NN,
        RNN,
        CNN,
        GRU,
        LSTM
    }
    public enum ENUM_NONLINEARY
    {
        SIGMOIDA,
        TANGENS
    }
    public enum ENUM_PARAMETERS
    {
        SPEED,
        MOMENT,
        NAME,
        RECURRENCE,
        LAYERS,
        EPOCH,
        TYPE,
        PROC_TRAIN,
        PROC_TEST,
        PROC_VALID
    }
    public enum ENUM_SPLIT_MODE
    {
        RANDOM,
        STEP,
        BLOCK_RIGHT,
        BLOCK_LEFT
    }
    public enum ENUM_DATA_TYPE
    {
        TRAIN,
        TEST
    }
    public class Classifire
    {
        public class Neuron     // Нейрон включает веса и выход
        {
            [IgnoreDataMember]
            public double
            A = 0,    // A  - выходной сигнал
            Ad = 0;   // Ad - прошлый выходной сигнал
            [DataMember]
            public List<double?> W;  // W  - текущие веса
            [IgnoreDataMember]
            public List<double?> Wd; // Wd - прошлые веса
            public Neuron() { }
            public Neuron(int count_W)
            {
                W = new List<double?>();
                Wd = new List<double?>();

                for (int i = 0; i < count_W; i++)
                {
                    W.Add(Math.RandomDouble(-2, 2));
                    Wd.Add(0);
                }
            }
        }
        public class Layer      // Слои состоят из нейронов
        {
            [DataMember]
            public List<Neuron> N = new List<Neuron>();
            public Layer() { }
            public Layer(int count_N, int count_W, bool Out = false)
            {
                for (int i = 0; i < count_N - 1; i++)
                {
                    N.Add(new Neuron(count_W));
                }

                if (Out)
                {
                    N.Add(new Neuron(0));
                }
                else
                {
                    N.Add(new Neuron(count_W));
                    N[count_N - 1].A = 1;
                }
            }
        }
        public List<Layer> layers = new List<Layer>();

        public class DataSetForJson
        {
            public class ParameterForJson
            {
                public ParameterForJson() { }
                public ParameterForJson(List<double> values, string name)
                {
                    this.values = values;
                    this.name = name;
                }

                public List<double> values;
                public string name;
            }

            public List<ParameterForJson>
            inputs = new List<ParameterForJson>(),      // Входные параметры
            outputs = new List<ParameterForJson>();     // Выходные параметры

            public void From(DataSet data)
            {
                for (int i = 0; i < data.inputs.Count; i++)
                {
                    inputs.Add(new ParameterForJson(data.inputs[i].orig, data.inputs[i].name));
                }

                for (int i = 0; i < data.outputs.Count; i++)
                {
                    outputs.Add(new ParameterForJson(data.outputs[i].orig, data.outputs[i].name));
                }
            }
        }
        public class DataSet
        {
            public class Parameter
            {
                public Parameter() { }
                public Parameter(string name) 
                {
                    this.name = name;
                }
                public Parameter(List<double> values)
                {
                    this.orig = values;
                }

                [IgnoreDataMember]
                public List<double> 
                orig = new List<double>(),  // Оригинальный набор данных
                norm = new List<double>();  // Нормализованный набор данных

                public double
                    math_expectation = 0,
                    dispersion = 0,
                    max = 0;
                public string name;

                public void Init()
                {
                    if (orig.Count <= 0) return;

                    norm.Clear();
                    norm = Math.Normalization(orig, ref math_expectation, ref dispersion, ref max);
                }
                public void Clear()
                {
                    orig.Clear();
                    norm.Clear();
                }
            }

            public List<Parameter> 
            inputs = new List<Parameter>(),      // Входные параметры
            outputs = new List<Parameter>();     // Выходные параметры

            [IgnoreDataMember]
            public List<ENUM_DATA_TYPE> data_types = new List<ENUM_DATA_TYPE>();

            public class Data
            {
                public List<double> inputs = new List<double>();
                public List<double> outs_norm = new List<double>();
                public List<double> outs_neuro = new List<double>();
                public List<double> outs_neuro_fact = new List<double>();
                public double loss;
                public int index;
            }

            [IgnoreDataMember]
            public List<Data> 
            train = new List<Data>(),  // Обучающие примеры (входа и выхода)
            test = new List<Data>();   // Тестовые примеры (входа и выхода)

            public int precision = 0;

            public ENUM_SPLIT_MODE separation_mode = ENUM_SPLIT_MODE.RANDOM;

            public double
                procent_train = 70,
                procent_test = 30;

            [IgnoreDataMember]
            public double
                error_train = 0,
                error_test = 0;

            public void Init() // После получения данных
            {
                for (int i = 0; i < inputs.Count; i++)
                {
                    inputs[i].Init();
                }

                for (int i = 0; i < outputs.Count; i++)
                {
                    outputs[i].Init();
                }

                // Если изменилось кол-во данных, проводим повторное распределение
                if(GetMinCount()!=data_types.Count) Separation();
                CreatingSets();
                SetPrecision();
            }
            public void Clear() // Перед получением новых данных
            {
                inputs.Clear();
                outputs.Clear();
                train.Clear();
                test.Clear();
            }
            public void DataDenorm(List<Data> data)
            {
                for (int i = 0; i < outputs.Count; i++)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        data[j].outs_neuro_fact[i] = Math.Denormalization(data[j].outs_neuro[i], outputs[i].math_expectation, outputs[i].dispersion, outputs[i].max);
                        data[j].outs_neuro_fact[i] = System.Math.Round(data[j].outs_neuro_fact[i], precision); // Огруглям до требуемой точности
                    }
                }
            }
            public void AddDataTypeByName(string value)
            {
                switch (value.ToUpper())
                {
                    case "TEST":
                        data_types.Add(ENUM_DATA_TYPE.TEST);
                        break;
                    default:
                        data_types.Add(ENUM_DATA_TYPE.TRAIN);
                        break;
                }
            }
            public string GetDataTypeNameByIndex(int index)
            {
                switch (data_types[index])
                {
                    case ENUM_DATA_TYPE.TEST:
                        return ("Test");
                    default:
                        return ("Train");
                }
            }
            public void Separation()
            {
                int count = GetMinCount();
                CorrectProcent();
                int count_test = (int)(count * (procent_test * 0.01));
                int count_train = count - count_test;

                Random rand = new Random();
                data_types.Clear();
                for (int i = 0; i < count; i++)
                {
                    switch (separation_mode)
                    {
                        case ENUM_SPLIT_MODE.RANDOM:
                            while (true)
                            {
                                ENUM_DATA_TYPE data_type = (ENUM_DATA_TYPE)rand.Next(3);
                                if (data_type == ENUM_DATA_TYPE.TRAIN && count_train > 0)
                                {
                                    data_types.Add(data_type);
                                    count_train--;
                                    break;
                                }
                                if (data_type == ENUM_DATA_TYPE.TEST && count_test > 0)
                                {
                                    data_types.Add(data_type);
                                    count_test--;
                                    break;
                                }
                                if (count_train == 0 && count_test == 0) break;
                            }
                            break;
                        case ENUM_SPLIT_MODE.STEP:
                            if (count_test > 0)
                            {
                                data_types.Add(ENUM_DATA_TYPE.TEST);
                                count_test--;
                            }
                            data_types.Add(ENUM_DATA_TYPE.TRAIN);
                            count_train--;
                            break;
                        case ENUM_SPLIT_MODE.BLOCK_LEFT:
                            while (count_test > 0)
                            {
                                data_types.Add(ENUM_DATA_TYPE.TEST);
                                count_test--;
                            }
                            while (count_train > 0)
                            {
                                data_types.Add(ENUM_DATA_TYPE.TRAIN);
                                count_train--;
                            }
                            break;
                        case ENUM_SPLIT_MODE.BLOCK_RIGHT:
                            while (count_train > 0)
                            {
                                data_types.Add(ENUM_DATA_TYPE.TRAIN);
                                count_train--;
                            }
                            while (count_test > 0)
                            {
                                data_types.Add(ENUM_DATA_TYPE.TEST);
                                count_test--;
                            }
                            break;
                    }
                }
            }                 // Распределяем на типы (train/test)
            private void CreatingSets()
            {
                train.Clear();
                test.Clear();
                for (int i = 0; i < data_types.Count; i++)
                {
                    switch (data_types[i])
                    {
                        case ENUM_DATA_TYPE.TRAIN:
                            train.Add(GetDataByIndex(i));
                            break;
                        case ENUM_DATA_TYPE.TEST:
                            test.Add(GetDataByIndex(i));
                            break;
                    }
                }
            }               // Создаем наборы (train/test) по распределению
            private Data GetDataByIndex(int index)
            {
                Data data = new Data();
                for (int j = 0; j < inputs.Count; j++)
                {
                    data.inputs.Add(inputs[j].norm[index]);
                }

                for (int j = 0; j < outputs.Count; j++)
                {
                    data.outs_norm.Add(outputs[j].norm[index]);
                    data.outs_neuro.Add(0);
                    data.outs_neuro_fact.Add(0);
                }
                data.index = index;
                return (data);
            }
            public int GetMinCount()
            {
                int count = 0;
                
                if (inputs.Count > 0) 
                    count = inputs[0].orig.Count;
                
                if (outputs.Count > 0 && count == 0) 
                    count = outputs[0].orig.Count; 

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (count > inputs[i].orig.Count)
                        count = inputs[i].orig.Count;
                }

                for (int i = 0; i < outputs.Count; i++)
                {
                    if (count > outputs[i].orig.Count)
                        count = outputs[i].orig.Count;
                }
                return (count);
            }
            public int GetMaxCount()
            {
                int count = 0;
                for (int i = 0; i < inputs.Count; i++)
                {
                    if (count < inputs[i].orig.Count)
                        count = inputs[i].orig.Count;
                }

                for (int i = 0; i < outputs.Count; i++)
                {
                    if (count < outputs[i].orig.Count)
                        count = outputs[i].orig.Count;
                }
                return (count);
            }
            private void CorrectProcent()
            {
                procent_train = System.Math.Abs(procent_train);
                procent_test = System.Math.Abs(procent_test);

                double proc_coef = (procent_train + procent_test) / 100;

                if (proc_coef == 0 || procent_train == 0 || procent_test == 0)
                {
                    procent_train = 70;
                    procent_test = 30;
                    return;
                }

                if (proc_coef == 1) return;

                if (proc_coef > 1)
                {
                    procent_train /= proc_coef;
                    procent_test /= proc_coef;
                    return;
                }

                if (proc_coef < 1)
                {
                    procent_train *= proc_coef;
                    procent_test *= proc_coef;
                }
            }
            private void SetPrecision()
            {
                int value=0;
                for (int i = 0; i < inputs.Count; i++)
                {
                    for (int j = 0; j < inputs[i].orig.Count; j++)
                    {
                        value=Math.GetPrecision(inputs[i].orig[j]);
                        if (value > precision) precision = value;
                    }
                }

                for (int i = 0; i < outputs.Count; i++)
                {
                    for (int j = 0; j < outputs[i].orig.Count; j++)
                    {
                        value = Math.GetPrecision(outputs[i].orig[j]);
                        if (value > precision) precision = value;
                    }
                }
            }
        }
        public DataSet data = new DataSet();

        [IgnoreDataMember]
        public Thread
            thread_create,
            thread_fit,
            thread_predict,
            thread_evaluate;

        public double
            speed = 0.5,
            moment = 0.5;
        public string 
            uuid = "HASH_MD5",
            name = "model#1",
            layers_hidden = "20,20";
        public ENUM_CLASSIFIRIES type;
        public DateTime
            time_create = DateTime.Now,
            time_last_fit = new DateTime(),
            time_last_save = new DateTime();
        public long epoch = 100000;

        [IgnoreDataMember]
        static public string[] type_names =
        {
         "NN",
         "RNN",
         "CNN",
         "GRU",
         "LSTM"
        };

        [IgnoreDataMember]
        public List<double> 
        errors_learn = new List<double>(),
        errors_test = new List<double>();

        public Classifire()
        {
            thread_fit = new Thread(new ParameterizedThreadStart(Fit));
            thread_create = new Thread(new ThreadStart(Create));
            thread_predict = new Thread(new ThreadStart(Predict));
            thread_evaluate = new Thread(new ThreadStart(Evaluate));

            Random rand = new Random();
            uuid = GetHashString(time_create.ToString() + rand.NextDouble().ToString());
            Console.WriteLine(uuid);
        }
        public static string GetHashString(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(text))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        public static byte[] GetHash(string text)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        public void CreateThread()
        {
            thread_create = new Thread(new ThreadStart(Create));
            thread_create.IsBackground = true;
            thread_create.Priority = ThreadPriority.AboveNormal;
            thread_create.Start();
        }
        public void FitThread(long epochs) 
        {
            time_last_fit = DateTime.Now;
            thread_fit = new Thread(new ParameterizedThreadStart(Fit));
            thread_fit.IsBackground = true;
            thread_fit.Priority = ThreadPriority.AboveNormal;
            thread_fit.Start(epochs);
        }
        public void PredictThread()
        {
            thread_predict = new Thread(new ThreadStart(Predict));
            thread_predict.IsBackground = true;
            thread_predict.Priority = ThreadPriority.AboveNormal;
            thread_predict.Start();
        }
        public void EvaluateThread()
        {
            thread_evaluate = new Thread(new ThreadStart(Evaluate));
            thread_evaluate.IsBackground = true;
            thread_evaluate.Priority = ThreadPriority.AboveNormal;
            thread_evaluate.Start();
        }

        public void Create() 
        { 
            data.Init();
            Configurate(); 
        }
        public virtual bool Configurate() { return (false); }
        public virtual void Fit(object epochs) { return; }
        public virtual void Predict() { return; }
        public virtual void Evaluate() { return; }
        public virtual ENUM_CLASSIFIRIES GetNeuroType() { return (ENUM_CLASSIFIRIES.NULL); }
        
        public void Clear() 
        {
            errors_learn.Clear();
            errors_test.Clear();
            data.Clear();
            //layers.Clear();
        }
        public void Set(ENUM_PARAMETERS parameter, object value) 
        {
            switch (parameter)
            {
                case ENUM_PARAMETERS.MOMENT:
                    moment = Convert.ToDouble(value);
                    break;
                case ENUM_PARAMETERS.SPEED:
                    speed = Convert.ToDouble(value);
                    break;
                case ENUM_PARAMETERS.EPOCH:
                    epoch = Convert.ToInt64(value);
                    break;
                case ENUM_PARAMETERS.NAME:
                    name = value.ToString();
                    break;
                case ENUM_PARAMETERS.TYPE:
                    type = (ENUM_CLASSIFIRIES)value;
                    break;
                case ENUM_PARAMETERS.LAYERS:
                    layers_hidden = value.ToString();
                    break;
                default:
                    break;
            } 
        }

        public void Save(string path = "") 
        {
            time_last_save = DateTime.Now;
            Console.WriteLine("save");
        }
        public void Load(string name, System.IO.Stream stream) 
        {
            Set(ENUM_PARAMETERS.NAME, name);
            Console.WriteLine("load");
        }

        public DataSetForJson GetDataSet()
        {
            DataSetForJson dataset = new DataSetForJson();
            dataset.From(data);
            return (dataset);
        }
        static public int GetIndexByUuid(List<Classifire> classifirs, string uuid)
        {
            for (int i = 0; i < classifirs.Count; i++)
            {
                if (classifirs[i].uuid == uuid) return (i);
            }
            return (-1);
        }
    }
}
