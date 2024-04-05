using CommunityToolkit.Mvvm.ComponentModel;
using ImageView.Model.DrawObject;
using ImageView.Model.ManagementSystem;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageView.ViewModel
{
    public class ImageViewViewModel : ObservableObject
    {
        private MainSystem _mainSystem = null;
        public ImageViewViewModel(MainSystem mainSystem)
        {
            _mainSystem = mainSystem;
            _mainSystem.ImageViewViewModel = this;

            _drawEllipse = new ObservableCollection<DrawEllipse>();
            _drawLine = new ObservableCollection<DrawLine>();
            _drawRect = new ObservableCollection<DrawRect>();
        }

        BitmapSource _mainImage;
        public BitmapSource MainImage
        {
            get { return _mainImage; }
            set
            {
                if (_mainImage != value)
                {
                    _mainImage = value;
                    OnPropertyChanged(nameof(MainImage));
                }
            }
        }

        public double Scale
        {
            get { return _mainSystem.Scale; }
            set
            {
                if (_mainSystem.Scale != value)
                {
                    _mainSystem.Scale = value;
                    OnPropertyChanged(nameof(Scale));
                    DeleteResult();
                    UpdateResult();
                }
            }
        }

        public double CenterPointX
        {
            get { return _mainSystem.CenterPointX; }
            set
            {
                if (_mainSystem.CenterPointX != value)
                {
                    _mainSystem.CenterPointX = value;
                    OnPropertyChanged(nameof(CenterPointX));
                }
            }
        }

        public double CenterPointY
        {
            get { return _mainSystem.CenterPointY; }
            set
            {
                if (_mainSystem.CenterPointY != value)
                {
                    _mainSystem.CenterPointY = value;
                    OnPropertyChanged(nameof(CenterPointY));
                }
            }
        }

        public double TranslationX
        {
            get { return _mainSystem.TranslationX; }
            set
            {
                if (_mainSystem.TranslationX != value)
                {
                    _mainSystem.TranslationX = value;
                    OnPropertyChanged(nameof(TranslationX));
                    DeleteResult();
                    UpdateResult();
                }
            }
        }

        public double TranslationY
        {
            get { return _mainSystem.TranslationY; }
            set
            {
                if (_mainSystem.TranslationY != value)
                {
                    _mainSystem.TranslationY = value;
                    OnPropertyChanged(nameof(TranslationY));
                    DeleteResult();
                    UpdateResult();
                }
            }
        }

        private ObservableCollection<DrawEllipse> _drawEllipse;
        public ObservableCollection<DrawEllipse> DrawEllipses
        {
            get { return _drawEllipse; }
            set
            {
                if (_drawEllipse != value)
                {
                    _drawEllipse = value;
                    OnPropertyChanged(nameof(DrawEllipses));
                }
            }
        }

        private ObservableCollection<DrawLine> _drawLine;
        public ObservableCollection<DrawLine> DrawLine
        {
            get { return _drawLine; }
            set
            {
                if (_drawLine != value)
                {
                    _drawLine = value;
                    OnPropertyChanged(nameof(DrawLine));
                }
            }
        }

        private ObservableCollection<DrawRect> _drawRect;
        public ObservableCollection<DrawRect> DrawRect
        {
            get { return _drawRect; }
            set
            {
                if (_drawRect != value)
                {
                    _drawRect = value;
                    OnPropertyChanged(nameof(DrawRect));
                }
            }
        }

        public void UpdateResult()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateEllipseResult();
                UpdateLineResult();
                UpdateRectResult();
            });
        }

        public void DeleteResult()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                DrawEllipses.Clear();
                DrawLine.Clear();
                DrawRect.Clear();
            });
        }

        void UpdateEllipseResult()
        {
            Queue<DrawEllipse> obj = _mainSystem.DrawObj.drawEllipses;
            foreach (DrawEllipse drawEllipse in obj) 
            {
                drawEllipse.UpdatePosition(Scale, TranslationX, TranslationY);
                DrawEllipses.Add(drawEllipse);
            }
        }

        void UpdateLineResult()
        {
            Queue<DrawLine> obj = _mainSystem.DrawObj.drawLines;

            foreach (DrawLine drawLines in obj)
                DrawLine.Add(drawLines);

            
        }

        void UpdateRectResult()
        {
            Queue<DrawRect> obj = _mainSystem.DrawObj.drawRects;

            foreach (DrawRect drawRects in obj)
                DrawRect.Add(drawRects);

            
        }
    }
}
