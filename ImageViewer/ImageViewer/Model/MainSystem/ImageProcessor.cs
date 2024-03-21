using System;
using OpenCvSharp;
using System;
using System.Numerics;

namespace ImageViewer.Model.MainSystem
{
    

    public class ImageProcessor
    {
        static ImageProcessor _intance;
        public static ImageProcessor Instance
        {
            get
            {
                if (_intance == null) _intance = new ImageProcessor();

                return _intance;
            }
        }

        private ImageProcessor(){}

        /// <summary>
        //////////////////////////////////////////////// Singleton
        /// </summary>
        
        Thread _processor;
        bool _IsProcessorRun = false;
        AutoResetEvent _AddItem;

        object _bufferLocker;

        Mat _buffer;
        Mat _bilateral;
        Mat _Threshold;
        Mat _Bolb;
        Mat _Result;

        Action<Mat> _callBackFunc;
        public void SetCallBackFunc(Action<Mat> CallBack) { _callBackFunc = CallBack; }

        public void InitImageProcessor() 
        {
            createBuffer();

            _bufferLocker = new object();

            _AddItem = new AutoResetEvent(false);

            _processor = new Thread(Run);
            _IsProcessorRun=true;
            _processor.Start();
        }

        void createBuffer() 
        {
           if(_buffer == null) _buffer = new Mat();
           if(_bilateral == null) _bilateral = new Mat();
           if(_Threshold == null) _Threshold = new Mat();
           if(_Bolb == null) _Bolb = new Mat();
            if (_Result == null) _Result = new Mat();
        }

        public void CloseImageProcessor() 
        {
            _IsProcessorRun = false;
            _processor.Join();

            if(_buffer != null) _buffer.Release();
            if (_bilateral != null) _bilateral.Release();
            if (_Threshold != null) _Threshold.Release();
            if (_Bolb != null) _Bolb.Release();
            if (_Result != null) _Result.Release();
        }
        void Run() 
        {
            while(_IsProcessorRun == true)
            {
                _AddItem.WaitOne();

                if (_buffer == null) continue;

                processDefect();


            }
            return;
        }

        void processDefect() 
        {
            //버퍼의 채널이 1이 아니라면 즉 grayscale 이 아닌 컬러일때 
            if (_buffer.Channels() != 1) 
                Cv2.CvtColor(_buffer, _buffer, ColorConversionCodes.BGR2GRAY);
            
            //노이즈 제거
            Cv2.BilateralFilter(_buffer, _bilateral, 5, 250, 10); 

            //객체 분리를 위해 Threshold
            Cv2.Threshold(_bilateral, _Threshold, 150, 255, ThresholdTypes.Binary);

            Point[][] contours;
            HierarchyIndex[] hierarchyIndex;

            //Blob의 외곽선 추출
            Cv2.FindContours(_Threshold, out contours, out hierarchyIndex, RetrievalModes.External, ContourApproximationModes.ApproxNone);

            List<CircleData> circleDatas = new List<CircleData>();

            Point[] CenterPoint = new Point[contours.Length];

            Cv2.CvtColor(_buffer, _Result, ColorConversionCodes.GRAY2BGR);
            for (int i = 0; i < contours.Length; ++i) 
            {
                int CoordiX = 0;
                int CoordiY = 0;

                for (int j = 0; j < contours[i].Length; ++j) 
                {
                    CoordiX += contours[i][j].X;
                    CoordiY += contours[i][j].Y;

                    Cv2.Circle(_Result, contours[i][j], 1, new Scalar(0, 255, 0), 1);
                }
                CenterPoint[i].X = CoordiX / contours[i].Length;
                CenterPoint[i].Y = CoordiY / contours[i].Length;

                CircleData circleData;

                //추출한 외곽선을 기준으로 circle Fitting
                FittingCircle(contours[i], out circleData);

                if (circleData == null) continue;
                circleDatas.Add(circleData);

                Cv2.Circle(_Result,(int)circleData.CenterPointX, (int)circleData.CenterPointY,
                    (int)circleData.Radius, new Scalar(0, 0, 255), 1);

                Cv2.Circle(_Result, new Point((int)circleData.CenterPointX, (int)circleData.CenterPointY), 1, new Scalar(0, 255,0), 5);

                Cv2.Circle(_Result, CenterPoint[i], 1, new Scalar(0, 0, 255), 3);
            }

            _callBackFunc(_Result);
        }

        void FittingCircle(Point[] point, out CircleData circle) 
        {
            int Length = point.Length;

            if (Length == 0)
            {
                circle = null;
                return;
            }
            
            Mat MatA = new Mat(Length,3, MatType.CV_64F);

            Mat vecB = new Mat(Length,1, MatType.CV_64F);

            for(int i = 0; i < Length; ++i)
            {
                MatA.At<double>(i, 0) = -2 * point[i].X;
                MatA.At<double>(i, 1) = -2 * point[i].Y;
                MatA.At<double>(i, 2) = 1;

                vecB.At<double>(i, 0) = - Math.Pow(point[i].X, 2) - Math.Pow(point[i].Y, 2);
            }

            Mat pseudoInverse = new Mat();

            Cv2.Invert(MatA, pseudoInverse, DecompTypes.SVD);

            Mat parameters = pseudoInverse * vecB;

            circle = new CircleData();
            circle.CenterPointX = parameters.At<double>(0, 0);
            circle.CenterPointY = parameters.At<double>(0, 1);
            circle.Radius = Math.Round(Math.Sqrt(Math.Pow(parameters.At<double>(0, 0), 2) + Math.Pow(parameters.At<double>(0, 1), 2) - parameters.At<double>(0, 2)),2);

            return;
        }

        public void SetImageProcessorBuffer(Mat Buffer) 
        {
            lock (_bufferLocker)
            {
                _buffer = Buffer;
                _AddItem.Set();
            }
        }
    }
}
