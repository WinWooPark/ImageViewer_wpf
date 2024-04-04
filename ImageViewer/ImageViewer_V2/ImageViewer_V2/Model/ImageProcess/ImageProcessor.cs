using ImageViewer_V2.Model.Data;
using ImageViewer_V2.Model.ManagementSystem;
using OpenCvSharp;
using System.Diagnostics;
using ImageViewer_V2.Model.ImageProcess.Data;
using System.Windows.Media;
using ImageViewer_V2.Define;

namespace ImageViewer_V2.Model.ImageProcess
{
    public class ImageProcessor
    {
        public ImageProcessor() {}

        MainSystem  _mainSystem;
        IntegratedClass _integratedClass;
        SystemData _systemData;
        BasicData _basicData;

        Thread _processor;
        bool _IsProcessorRun = false;
        AutoResetEvent _addItem;

        bool _IsInspRun = false;

        object _bufferLocker;

        Mat _buffer;
        Mat _bilateral;
        Mat _Threshold;
        Mat _Bolb;
        Mat _result;
        Stopwatch _stopWatch;

        public Mat Result { get { return _result; } }

        public void InitImageProcessor(IntegratedClass integratedClass)
        {
            _mainSystem = MainSystem.Instance;
            _integratedClass = integratedClass;
            _systemData = _integratedClass.SystemData;
            _basicData = _integratedClass.BasicData;

            createBuffer();

            _stopWatch = new Stopwatch();

            _bufferLocker = new object();

            _addItem = new AutoResetEvent(false);

            _processor = new Thread(Run);
            _IsProcessorRun = true;
            _processor.Start();
        }

        void createBuffer()
        {
            if (_buffer == null) _buffer = new Mat();
            if (_bilateral == null) _bilateral = new Mat();
            if (_Threshold == null) _Threshold = new Mat();
            if (_Bolb == null) _Bolb = new Mat();
            if (_result == null) _result = new Mat();
        }

        public void CloseImageProcessor()
        {
            _IsProcessorRun = false;
            _addItem.Set();
            _processor.Join();

            if (_buffer != null) _buffer.Release();
            if (_bilateral != null) _bilateral.Release();
            if (_Threshold != null) _Threshold.Release();
            if (_Bolb != null) _Bolb.Release();
            if (_result != null) _result.Release();
        }
        void Run()
        {
            while (_IsProcessorRun == true)
            {

                if (_IsInspRun == true)
                {
                    if (!_buffer.Empty())
                        processDefect();

                    _IsInspRun = false;
                }
                else _addItem.WaitOne();

            }
            return;
        }

        void processDefect()
        {
            _stopWatch.Reset();
            _stopWatch.Start();

            //버퍼의 채널이 1이 아니라면 즉 grayscale 이 아닌 컬러일때 
            if (_buffer.Channels() != 1)
                Cv2.CvtColor(_buffer, _buffer, ColorConversionCodes.BGR2GRAY);

            //노이즈 제거
            Cv2.BilateralFilter(_buffer, _bilateral, 5, 250, 10);

            int nThreshold = _basicData.Threshold;

            //객체 분리를 위해 Threshold
            Cv2.Threshold(_bilateral, _Threshold, nThreshold, 255, ThresholdTypes.Binary);

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchyIndex;

            //Blob의 외곽선 추출
            Cv2.FindContours(_Threshold, out contours, out hierarchyIndex, RetrievalModes.External, ContourApproximationModes.ApproxNone);

            OpenCvSharp.Point[] CenterPoint = new OpenCvSharp.Point[contours.Length];

            for (int i = 0; i < contours.Length; ++i)
            {
                BlobData blobData = null;

                //추출한 외곽선을 기준으로 circle Fitting
                FittingCircle(contours[i], out blobData);

                if (blobData != null)
                {
                    blobData.Index = i;
                    blobData.Result = Judgement(blobData);

                    //데이터에 넣어둔다
                    _systemData.BlobDatas.Enqueue(blobData);
                }
            }

            //Parallel.For(0, contours.Length, (i =>
            //{
            //    BlobData blobData = null;

            //    //추출한 외곽선을 기준으로 circle Fitting
            //    FittingCircle(contours[i], out blobData);

            //    if (blobData != null)
            //    {
            //        blobData.Index = i;
            //        blobData.Result = Judgement(blobData);

            //        //데이터에 넣어둔다
            //       _systemData.BlobDatas.Enqueue(blobData);
            //    }
            //}));

            DrawResult();

            _stopWatch.Stop();
            double ProcessTime = _stopWatch.Elapsed.TotalMilliseconds;
           
            _mainSystem.CBcoredoneInspction(ProcessTime);
        }

        void DrawResult()
        {
            Cv2.CvtColor(_buffer, _result, ColorConversionCodes.GRAY2BGR);
            foreach (BlobData blobData in _systemData.BlobDatas)
            {
                _integratedClass.ImageViewAPI.GetDrawObjectEllipse(blobData.LeftTopPoint.X, blobData.LeftTopPoint.Y, blobData.BlobSize.Width, blobData.BlobSize.Height, blobData.Result);
            }
        }

        void FittingCircle(OpenCvSharp.Point[] point, out BlobData blobData)
        {
            int Length = point.Length;

            if (Length == 0)
            {
                blobData = null;
                return;
            }

            Mat MatA = new Mat(Length, 3, MatType.CV_64F);

            Mat vecB = new Mat(Length, 1, MatType.CV_64F);

            for (int i = 0; i < Length; ++i)
            {
                MatA.At<double>(i, 0) = -2 * point[i].X;
                MatA.At<double>(i, 1) = -2 * point[i].Y;
                MatA.At<double>(i, 2) = 1;

                vecB.At<double>(i, 0) = -Math.Pow(point[i].X, 2) - Math.Pow(point[i].Y, 2);
            }

            //Parallel.For(0, Length, (i =>
            //{
            //    MatA.At<double>(i, 0) = -2 * point[i].X;
            //    MatA.At<double>(i, 1) = -2 * point[i].Y;
            //    MatA.At<double>(i, 2) = 1;

            //    vecB.At<double>(i, 0) = -Math.Pow(point[i].X, 2) - Math.Pow(point[i].Y, 2);
            //}));

            Mat pseudoInverse = new Mat();

            Cv2.Invert(MatA, pseudoInverse, DecompTypes.SVD);

            Mat parameters = pseudoInverse * vecB;

            blobData = new BlobData();
            blobData.CenterPointX = parameters.At<double>(0);
            blobData.CenterPointY = parameters.At<double>(1);
            blobData.Radius = Math.Round(Math.Sqrt(Math.Pow(parameters.At<double>(0), 2) + Math.Pow(parameters.At<double>(1), 2) - parameters.At<double>(2)), 2);
            blobData.CalBlobSize();
            return;
        }

        bool Judgement(BlobData blobData)
        {
            double reference = _basicData.Reference;

            //if (blobData.Width >= reference || blobData.Height >= reference) return false;
            if (blobData.Radius >= reference) return false;
            else return true;
        }

        public void SetImageProcessorBuffer(Mat Buffer)
        {
            lock (_bufferLocker)
            {
                _buffer = Buffer;
                _IsInspRun = true;
                _addItem.Set();
            }
        }

        public Mat SelectedImage(string str)
        {
            Mat SelectImage = null;
            switch (str)
            {
                case "Original":
                    SelectImage = _buffer;
                    break;
                case "Filter":
                    SelectImage = _bilateral;
                    break;
                case "Threshold":
                    SelectImage = _Threshold;
                    break;
                case "Bolb":
                    SelectImage = _result;
                    break;
                default:
                    SelectImage = _result;
                    break;
            }
            return SelectImage;
        }
    }
}
