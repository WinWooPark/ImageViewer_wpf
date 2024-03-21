using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ImageViewer.Behaviors
{
    public class MouseDragBehavior : Behavior<Image>
    {
        private TranslateTransform _translateTransform;
        protected override void OnAttached()
        {
            //AssoicatedObject의 이벤트 핸들러 추가
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseDown;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseUp;
            AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;

            var transformGroup = AssociatedObject.LayoutTransform as TransformGroup;
            if (transformGroup != null)
            {
                _translateTransform = transformGroup.Children[0] as TranslateTransform;
                if (_translateTransform == null)
                {
                    _translateTransform = new TranslateTransform();
                    transformGroup.Children.Insert(0, _translateTransform);
                }
            }
            else
            {
                _translateTransform = new TranslateTransform();
                AssociatedObject.LayoutTransform = _translateTransform;
            }
        }

        protected override void OnDetaching()
        {
            //이벤트 핸들러 삭제
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseDown;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseUp;
            AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
        }

        double _translateX;
        public double TranslateX 
        {
            get { return _translateX; }
            set 
            {
                if (_translateX != value) 
                {
                    _translateX = value;
                }
            }
        }
        double _translateY;
        public double TranslateY
        {
            get { return _translateY; }
            set
            {
                if (_translateY != value)
                {
                    _translateY = value;
                }
            }
        }

        private double _currentScale = 0.1;
        private const double ScaleStep = 0.1;
        private bool _isDragging = false;
        private Point _startPoint;


        private double _scale = 1.0;
        private Point _relativePosition;
        private void AssociatedObject_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            //var scaleTransform = AssociatedObject.LayoutTransform as ScaleTransform;

            //if (scaleTransform == null)
            //{
            //    scaleTransform = new ScaleTransform();
            //    AssociatedObject.LayoutTransform = scaleTransform;
            //}

            //if (e.Delta > 0) 
            //{
            //    _currentScale += ScaleStep;
            //}
            //else 
            //{
            //    _currentScale -= ScaleStep;
            //    if (_currentScale < ScaleStep)
            //        _currentScale = ScaleStep;
            //}

            //scaleTransform.ScaleX = _currentScale;
            //scaleTransform.ScaleY = _currentScale;
            var image = AssociatedObject;

            // 이미지의 현재 위치를 가져옴
            var currentPosition = e.GetPosition(image);

            // 이미지를 확대 또는 축소할 수 있도록 scale 값을 조절
            _scale += e.Delta > 0 ? 0.1 : -0.1;
            _scale = Math.Max(0.1, _scale); // 최소 스케일 제한

            // 이미지의 스케일을 변경함
            image.LayoutTransform = new ScaleTransform(_scale, _scale);

            // 이미지의 중심을 기준으로 스케일링하기 위해 중심 위치를 계산
            var relativePosition = image.TranslatePoint(_relativePosition, image);

            // 중심 위치를 기준으로 이미지를 이동시킴
            image.TranslatePoint(new Point(relativePosition.X / image.ActualWidth, relativePosition.Y / image.ActualHeight), image);

            // 이전의 상대적 위치를 업데이트
            _relativePosition = currentPosition;
        }

        private void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(AssociatedObject);
        }
        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {            

            if (_isDragging == true) 
            {
                Point currentPosition = e.GetPosition(AssociatedObject);
                Vector diff = _startPoint - currentPosition;

                // TranslateTransform을 사용하여 이미지를 이동시킴
                _translateTransform.X -= diff.X;
                _translateTransform.Y -= diff.Y;

                _startPoint = currentPosition;
            }
        }

        private void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDragging = false;
        }

        public MouseDragBehavior()
        {
             _isDragging = false;
        }

        Action<double, double> _mouseDragCallBack;

        public void SetCallBack(Action<double, double> mouseDragCallBack) { _mouseDragCallBack = mouseDragCallBack; }

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(MouseDragBehavior), new PropertyMetadata(null));
    }
}
