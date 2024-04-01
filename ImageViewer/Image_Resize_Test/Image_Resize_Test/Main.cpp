#include "Main.h"

typedef struct BlobData 
{
    int _index;         //Blob 의 인덱스
    bool _result;       //Blob 결과 false = 불양 true = 양품
    double _radius;
    Point2d _centerPoint;

    void Reset() 
    {
        this->_centerPoint = Point2d(.0,.0);
        this->_index = 0;
        this->_radius = 0;
        this->_result = false;
    }
}BlobData;

void FittingCircle(vector<Point>& point, BlobData &blobData)
{
    int Length = point.size();

    Mat MatA(Length, 3, CV_64F);

    Mat vecB(Length, 1, CV_64F);

    for (int i = 0; i < Length; ++i)
    {
        MatA.at<double>(i, 0) = -2 * point[i].x;
        MatA.at<double>(i, 1) = -2 * point[i].y;
        MatA.at<double>(i, 2) = 1;

        vecB.at<double>(i, 0) = -pow(point[i].x, 2) - pow(point[i].y, 2);
    }

    Mat pseudoInverse = MatA.inv(DECOMP_SVD);

    Mat parameters = pseudoInverse * vecB;

    blobData.Reset();
    blobData._centerPoint.x = parameters.at<double>(0);
    blobData._centerPoint.y = parameters.at<double>(1);
    blobData._radius = sqrt(pow(parameters.at<double>(0), 2) + pow(parameters.at<double>(1), 2) - parameters.at<double>(2));
    
    return;
}

enum ImageType { eOrigin = 0, eResize, ImageTypeCount };

int main()
{
    Mat _buffer;
    Mat _Resize;
    Mat _bilateral;
    Mat _Threshold;
    Mat _result[ImageTypeCount];
    Mat _Draw = cv::Mat::zeros(822, 832.5, CV_8UC3);

    vector<BlobData> _blobData[ImageTypeCount];

    _buffer = imread("../TEST.bmp",0);
    if (_buffer.empty()) return -1;

    cv::resize(_buffer, _Resize, Size(830, 830));

    cv::imwrite("Resize.bmp", _Resize);

    cv::bilateralFilter(_buffer, _bilateral, 5, 250, 10);
   

	int nThreshold = 150;

	cv::threshold(_bilateral, _Threshold, nThreshold, 255, THRESH_BINARY);
 
    cv::resize(_Threshold, _Resize, Size(822, 822));

	vector<vector<Point>> contours[ImageTypeCount];
	

	findContours(_Threshold, contours[eOrigin], RETR_EXTERNAL, CHAIN_APPROX_NONE);
    findContours(_Resize, contours[eResize], RETR_EXTERNAL, CHAIN_APPROX_NONE);

    for (auto iter = contours[eOrigin].begin(); iter != contours[eOrigin].end(); ++iter)
    {
        BlobData Blob;
        FittingCircle((*iter), Blob);

        _blobData[eOrigin].push_back(Blob);
    }

    for (auto iter = contours[eResize].begin(); iter != contours[eResize].end(); ++iter)
    {
        BlobData Blob;
        FittingCircle((*iter), Blob);

        _blobData[eResize].push_back(Blob);
    }

    double SacleX = 822.0 / _buffer.cols;
    double SacleY = 822.0 / _buffer.rows;

    cv::cvtColor(_buffer, _result[eOrigin], COLOR_GRAY2BGR);
    cv::cvtColor(_Resize, _result[eResize], COLOR_GRAY2BGR);

    vector<Point2d> DrawPoints;

    for (int i = 0; i < _blobData[eOrigin].size(); ++i) 
    {
        Point pt;

        pt.x = _blobData[eOrigin][i]._centerPoint.x * SacleX;
        pt.y = _blobData[eOrigin][i]._centerPoint.y * SacleY;

        Point2d ptd;
        ptd.x = _blobData[eResize][i]._centerPoint.x / _blobData[eOrigin][i]._centerPoint.x;
        ptd.y = _blobData[eResize][i]._centerPoint.y / _blobData[eOrigin][i]._centerPoint.y;

        Size2d Size;
        Size.width = (_Draw.rows - _result[eResize].rows) / 2;
        Size.height = (_Draw.cols - _result[eResize].cols) / 2;


        Point2d DrawPoint;
        DrawPoint.x = pt.x + Size.width;
        DrawPoint.y = pt.y + Size.height;

        printf("원본좌표 X : %0.3f  , Y : %0.3f \n리사이즈 X : %0.3f  , Y : %0.3f\n변환좌표 X : %d  , Y : %d\n스케일 X : %0.3f  , %0.3f Y : %0.3f , %0.3f\nCanvas Point X : %0.3f , Y : %0.3f \n\n",
            _blobData[eOrigin][i]._centerPoint.x, _blobData[eOrigin][i]._centerPoint.y,
            _blobData[eResize][i]._centerPoint.x, _blobData[eResize][i]._centerPoint.y,
            pt.x, pt.y,
            SacleX, ptd.x, SacleY, ptd.y,
            DrawPoint.x, DrawPoint.y);


        circle(_result[eOrigin], Point(_blobData[eOrigin][i]._centerPoint.x, _blobData[eOrigin][i]._centerPoint.y), 1, Scalar(0, 0, 255), 1, 8, 0);
        circle(_result[eResize], Point(_blobData[eResize][i]._centerPoint.x, _blobData[eResize][i]._centerPoint.y), 1, Scalar(0, 0, 255), 1, 8, 0);
        circle(_result[eResize], Point(_blobData[eResize][i]._centerPoint.x, _blobData[eResize][i]._centerPoint.y), _blobData[eResize][i]._radius, Scalar(0, 0, 255), 1, 8, 0);
        circle(_result[eResize], pt, 1, Scalar(0, 255,0), 1, 8, 0);
        circle(_result[eResize], pt, _blobData[eResize][i]._radius, Scalar(0, 255, 0), 1, 8, 0);

       

        circle(_Draw, DrawPoint, _blobData[eResize][i]._radius, Scalar(0, 255, 0), 1, 8, 0);
        DrawPoints.push_back(DrawPoint);

    }
    

  /*  for (auto iter = DrawPoints.begin(); iter != DrawPoints.end(); ++iter) 
    {
        printf("Canvas Point X : %0.3f , Y : %0.3f\n", iter->x, iter->y);
    }*/
    
    cv::imwrite("Draw.bmp", _Draw);
    cv::imwrite("_resulteResize.bmp", _result[eOrigin]);
    cv::imwrite("_resulteOrigin.bmp", _result[eResize]);



	return 0;
}