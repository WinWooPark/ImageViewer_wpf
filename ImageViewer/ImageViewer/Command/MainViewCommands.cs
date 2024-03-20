﻿using ImageViewer.Model.MainSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewer.Command
{
    public class CommandsImageLoad : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsImageLoad(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ImageRoad();
        }
    }

    public class CommandsImageSave : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsImageSave(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ImageSave("E:\\5. Project_Test\\3. WPF_ImageViewer\\ImageViewer\\TEST.bmp");
        }
    }

    public class CommandsImageChange : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsImageChange(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ImageRoad();
        }
    }
    public class CommandsStart : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsStart(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ImageRoad();
        }
    }

    public class CommandsImageFit : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsImageFit(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ImageRoad();
        }
    }

    public class CommandsImageZoomIn : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsImageZoomIn(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ZoomIn();
        }
    }

    public class CommandsImageZoomOut : CommandBase
    {
        private SystemInfo _systemInfo;
        public CommandsImageZoomOut(SystemInfo systemInfo)
        {
            _systemInfo = systemInfo;
        }
        public override void Execute(object? parameter)
        {
            _systemInfo.ZoomOut();
        }
    }
}
