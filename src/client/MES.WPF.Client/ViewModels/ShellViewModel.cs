using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using MES.Client.Core.Contracts.Services;
using MES.Client.Core.Models;
using MES.WPF.Client.Helpers;
using MES.WPF.Client.Properties;

namespace MES.WPF.Client.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        private ICommand _optionsMenuItemInvokedCommand;

        private readonly INavigationService _navigationService;
        private RelayCommand _goBackCommand;
        private ICommand _menuItemInvokedCommand;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;
        
        [ObservableProperty]
        private object _selectedMenuItem;
        
        partial void OnSelectedMenuItemChanged(object? oldValue, object newValue)
        {
            if (newValue is PageMetadata { TargetType: not null } navigationPaneItem)
            {
                NavigateTo(navigationPaneItem.TargetType);
            }
        }

        public void UpdateFillColor(SolidColorBrush FillColor)
        {
            foreach (var item in MenuItems)
            {
                (item as PageMetadata).Path.Fill = FillColor;
            }

            SetttingsIconColor = FillColor;
        }

        private SolidColorBrush setttingsIconColor;

        public SolidColorBrush SetttingsIconColor
        {
            get { return setttingsIconColor; }
            set
            {
                setttingsIconColor = value;
                OnPropertyChanged(nameof(SetttingsIconColor));
            }
        }

        // TODO WTS: Change the icons and titles for all HamburgerMenuItems here.
        public ObservableCollection<PageMetadata> MenuItems { get; set; } = new ObservableCollection<PageMetadata>()
        {
            new PageMetadata()
            {
                Label = Resources.ShellKanbanPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V6C46 3.79086 44.2091 2 42 2H6ZM5 5C5 4.44772 5.44772 4 6 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H6C5.44772 6 5 5.55228 5 5ZM18 5C18 4.44772 18.4477 4 19 4H29C29.5523 4 30 4.44772 30 5C30 5.55228 29.5523 6 29 6H19C18.4477 6 18 5.55228 18 5ZM32 5C32 4.44772 32.4477 4 33 4H42C42.5523 4 43 4.44772 43 5C43 5.55228 42.5523 6 42 6H33C32.4477 6 32 5.55228 32 5ZM5 10.5C5 9.11929 6.11929 8 7.5 8H13.5C14.8807 8 16 9.11929 16 10.5V13.5C16 14.8807 14.8807 16 13.5 16H7.5C6.11929 16 5 14.8807 5 13.5V10.5ZM7.5 10C7.22386 10 7 10.2239 7 10.5V13.5C7 13.7761 7.22386 14 7.5 14H13.5C13.7761 14 14 13.7761 14 13.5V10.5C14 10.2239 13.7761 10 13.5 10H7.5ZM18 10.5C18 9.11929 19.1193 8 20.5 8H27.5C28.8807 8 30 9.11929 30 10.5V16.5C30 17.8807 28.8807 19 27.5 19H20.5C19.1193 19 18 17.8807 18 16.5V10.5ZM20.5 10C20.2239 10 20 10.2239 20 10.5V16.5C20 16.7761 20.2239 17 20.5 17H27.5C27.7761 17 28 16.7761 28 16.5V10.5C28 10.2239 27.7761 10 27.5 10H20.5ZM32 10.5C32 9.11929 33.1193 8 34.5 8H40.5C41.8807 8 43 9.11929 43 10.5V11.5C43 12.8807 41.8807 14 40.5 14H34.5C33.1193 14 32 12.8807 32 11.5V10.5ZM34.5 10C34.2239 10 34 10.2239 34 10.5V11.5C34 11.7761 34.2239 12 34.5 12H40.5C40.7761 12 41 11.7761 41 11.5V10.5C41 10.2239 40.7761 10 40.5 10H34.5ZM32 18.5C32 17.1193 33.1193 16 34.5 16H40.5C41.8807 16 43 17.1193 43 18.5V21.5C43 22.8807 41.8807 24 40.5 24H34.5C33.1193 24 32 22.8807 32 21.5V18.5ZM34.5 18C34.2239 18 34 18.2239 34 18.5V21.5C34 21.7761 34.2239 22 34.5 22H40.5C40.7761 22 41 21.7761 41 21.5V18.5C41 18.2239 40.7761 18 40.5 18H34.5ZM5 20.5C5 19.1193 6.11929 18 7.5 18H13.5C14.8807 18 16 19.1193 16 20.5V25.5C16 26.8807 14.8807 28 13.5 28H7.5C6.11929 28 5 26.8807 5 25.5V20.5ZM7.5 20C7.22386 20 7 20.2239 7 20.5V25.5C7 25.7761 7.22386 26 7.5 26H13.5C13.7761 26 14 25.7761 14 25.5V20.5C14 20.2239 13.7761 20 13.5 20H7.5ZM18 23.5C18 22.1193 19.1193 21 20.5 21H27.5C28.8807 21 30 22.1193 30 23.5V32.5C30 33.8807 28.8807 35 27.5 35H20.5C19.1193 35 18 33.8807 18 32.5V23.5ZM20.5 23C20.2239 23 20 23.2239 20 23.5V32.5C20 32.7761 20.2239 33 20.5 33H27.5C27.7761 33 28 32.7761 28 32.5V23.5C28 23.2239 27.7761 23 27.5 23H20.5ZM32 28.5C32 27.1193 33.1193 26 34.5 26H40.5C41.8807 26 43 27.1193 43 28.5V31.5C43 32.8807 41.8807 34 40.5 34H34.5C33.1193 34 32 32.8807 32 31.5V28.5ZM34.5 28C34.2239 28 34 28.2239 34 28.5V31.5C34 31.7761 34.2239 32 34.5 32H40.5C40.7761 32 41 31.7761 41 31.5V28.5C41 28.2239 40.7761 28 40.5 28H34.5ZM32 38.5C32 37.1193 33.1193 36 34.5 36H40.5C41.8807 36 43 37.1193 43 38.5V41.5C43 42.8807 41.8807 44 40.5 44H34.5C33.1193 44 32 42.8807 32 41.5V38.5ZM34.5 38C34.2239 38 34 38.2239 34 38.5V41.5C34 41.7761 34.2239 42 34.5 42H40.5C40.7761 42 41 41.7761 41 41.5V38.5C41 38.2239 40.7761 38 40.5 38H34.5Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(KanbanViewModel)
            },
            new PageMetadata()
            {
                Label = Resources.ShellMainPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M28.414 4H7V44H39V14.586ZM29 7.414 35.586 14H29ZM9 42V6H27V16H37V42Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(MainViewModel)
            },
            new PageMetadata()
            {
                Label = Resources.ShellPropertyGridPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(PropertyGridViewModel)
            },
            new PageMetadata()
            {
                Label = Resources.ShellTileViewPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 3.5C0 1.567 1.567 0 3.5 0H10.5C12.433 0 14 1.567 14 3.5V26.5C14 28.433 12.433 30 10.5 30H3.5C1.567 30 0 28.433 0 26.5V3.5ZM3.5 2C2.67157 2 2 2.67157 2 3.5V26.5C2 27.3284 2.67157 28 3.5 28H10.5C11.3284 28 12 27.3284 12 26.5V3.5C12 2.67157 11.3284 2 10.5 2H3.5ZM17 20.5C17 18.567 18.567 17 20.5 17H27.5C29.433 17 31 18.567 31 20.5V27.5C31 29.433 29.433 31 27.5 31H20.5C18.567 31 17 29.433 17 27.5V20.5ZM20.5 19C19.6716 19 19 19.6716 19 20.5V27.5C19 28.3284 19.6716 29 20.5 29H27.5C28.3284 29 29 28.3284 29 27.5V20.5C29 19.6716 28.3284 19 27.5 19H20.5ZM0 36.5C0 34.567 1.567 33 3.5 33H10.5C12.433 33 14 34.567 14 36.5V44.5C14 46.433 12.433 48 10.5 48H3.5C1.567 48 0 46.433 0 44.5V36.5ZM3.5 35C2.67157 35 2 35.6716 2 36.5V44.5C2 45.3284 2.67157 46 3.5 46H10.5C11.3284 46 12 45.3284 12 44.5V36.5C12 35.6716 11.3284 35 10.5 35H3.5ZM20.5 0C18.567 0 17 1.567 17 3.5V10.5C17 12.433 18.567 14 20.5 14H27.5C29.433 14 31 12.433 31 10.5V3.5C31 1.567 29.433 0 27.5 0H20.5ZM19 3.5C19 2.67157 19.6716 2 20.5 2H27.5C28.3284 2 29 2.67157 29 3.5V10.5C29 11.3284 28.3284 12 27.5 12H20.5C19.6716 12 19 11.3284 19 10.5V3.5ZM34 3.5C34 1.567 35.567 0 37.5 0H44.5C46.433 0 48 1.567 48 3.5V15.5C48 17.433 46.433 19 44.5 19H37.5C35.567 19 34 17.433 34 15.5V3.5ZM37.5 2C36.6716 2 36 2.67157 36 3.5V15.5C36 16.3284 36.6716 17 37.5 17H44.5C45.3284 17 46 16.3284 46 15.5V3.5C46 2.67157 45.3284 2 44.5 2H37.5ZM34 25.5C34 23.567 35.567 22 37.5 22H44.5C46.433 22 48 23.567 48 25.5V44.5C48 46.433 46.433 48 44.5 48H37.5C35.567 48 34 46.433 34 44.5V25.5ZM37.5 24C36.6716 24 36 24.6716 36 25.5V44.5C36 45.3284 36.6716 46 37.5 46H44.5C45.3284 46 46 45.3284 46 44.5V25.5C46 24.6716 45.3284 24 44.5 24H37.5ZM20.5 34C18.567 34 17 35.567 17 37.5V44.5C17 46.433 18.567 48 20.5 48H27.5C29.433 48 31 46.433 31 44.5V37.5C31 35.567 29.433 34 27.5 34H20.5ZM19 37.5C19 36.6716 19.6716 36 20.5 36H27.5C28.3284 36 29 36.6716 29 37.5V44.5C29 45.3284 28.3284 46 27.5 46H20.5C19.6716 46 19 45.3284 19 44.5V37.5Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(TileViewViewModel)
            },
            new PageMetadata()
            {
                Label = Resources.ShellTreeGridPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V14V42C48 45.3137 45.3137 48 42 48H13H6C2.68629 48 0 45.3137 0 42V14V6ZM6 2C3.79086 2 2 3.79086 2 6V13H13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H13H2V42C2 44.2091 3.79086 46 6 46H13H42C44.2091 46 46 44.2091 46 42V15ZM15 23C15 22.4477 15.4477 22 16 22H34C34.5523 22 35 22.4477 35 23C35 23.5523 34.5523 24 34 24H16C15.4477 24 15 23.5523 15 23ZM21 29C20.4477 29 20 29.4477 20 30C20 30.5523 20.4477 31 21 31H39C39.5523 31 40 30.5523 40 30C40 29.4477 39.5523 29 39 29H21ZM24 37C24 36.4477 24.4477 36 25 36H41C41.5523 36 42 36.4477 42 37C42 37.5523 41.5523 38 41 38H25C24.4477 38 24 37.5523 24 37ZM10.8 23.9333C10.4 24.4667 9.6 24.4667 9.2 23.9333L8.2 22.6C7.70557 21.9408 8.17595 21 9 21H11C11.824 21 12.2944 21.9408 11.8 22.6L10.8 23.9333ZM15.8 30.9333L16.8 29.6C17.2944 28.9408 16.824 28 16 28H14C13.176 28 12.7056 28.9408 13.2 29.6L14.2 30.9333C14.6 31.4667 15.4 31.4667 15.8 30.9333ZM20.9333 36.2C21.4667 36.6 21.4667 37.4 20.9333 37.8L19.6 38.8C18.9408 39.2944 18 38.824 18 38V36C18 35.176 18.9408 34.7056 19.6 35.2L20.9333 36.2Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(TreeGridViewModel)
            },
            new PageMetadata()
            {
                Label = Resources.ShellTreeViewPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M42.5 6C43.8807 6 45 4.88071 45 3.5V2.5C45 1.11929 43.8807 0 42.5 0H13.5C12.1193 0 11 1.11929 11 2.5V3.5C11 4.88071 12.1193 6 13.5 6H42.5ZM43 3.5C43 3.77614 42.7761 4 42.5 4H13.5C13.2238 4 13 3.77614 13 3.5V2.5C13 2.22386 13.2238 2 13.5 2H42.5C42.7761 2 43 2.22386 43 2.5V3.5ZM42.5 17C43.8807 17 45 15.8807 45 14.5V13.5C45 12.1193 43.8807 11 42.5 11H18.5C17.1193 11 16 12.1193 16 13.5V14.5C16 15.8807 17.1193 17 18.5 17H42.5ZM43 14.5C43 14.7761 42.7761 15 42.5 15H18.5C18.2238 15 18 14.7761 18 14.5V13.5C18 13.2239 18.2238 13 18.5 13H42.5C42.7761 13 43 13.2239 43 13.5V14.5ZM45 27.5C45 28.8807 43.8807 30 42.5 30H18.5C17.1193 30 16 28.8807 16 27.5V26.5C16 25.1193 17.1193 24 18.5 24H42.5C43.8807 24 45 25.1193 45 26.5V27.5ZM42.5 28C42.7761 28 43 27.7761 43 27.5V26.5C43 26.2239 42.7761 26 42.5 26H18.5C18.2238 26 18 26.2239 18 26.5V27.5C18 27.7761 18.2238 28 18.5 28H42.5ZM42.5 42C43.8807 42 45 40.8807 45 39.5V38.5C45 37.1193 43.8807 36 42.5 36H13.5C12.1193 36 11 37.1193 11 38.5V39.5C11 40.8807 12.1193 42 13.5 42H42.5ZM43 39.5C43 39.7761 42.7761 40 42.5 40H13.5C13.2238 40 13 39.7761 13 39.5V38.5C13 38.2239 13.2238 38 13.5 38H42.5C42.7761 38 43 38.2239 43 38.5V39.5ZM4.83204 5.25192C4.43621 5.84566 3.56376 5.84566 3.16794 5.25192L1.03645 2.0547C0.593416 1.39015 1.06981 0.5 1.8685 0.5L6.13147 0.5C6.93016 0.5 7.40656 1.39015 6.96352 2.0547L4.83204 5.25192ZM11.7519 15.3321C12.3456 14.9362 12.3456 14.0638 11.7519 13.6679L8.55469 11.5365C7.89013 11.0934 6.99999 11.5698 6.99999 12.3685L6.99998 16.6315C6.99998 17.4302 7.89013 17.9066 8.55468 17.4635L11.7519 15.3321ZM11.7519 26.6679C12.3456 27.0638 12.3456 27.9362 11.7519 28.3321L8.55468 30.4635C7.89013 30.9066 6.99998 30.4302 6.99998 29.6315L6.99999 25.3685C6.99999 24.5698 7.89013 24.0934 8.55469 24.5365L11.7519 26.6679ZM3.16794 41.2519C3.56376 41.8457 4.43621 41.8457 4.83204 41.2519L6.96352 38.0547C7.40656 37.3901 6.93016 36.5 6.13147 36.5H1.8685C1.06981 36.5 0.593416 37.3901 1.03645 38.0547L3.16794 41.2519Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(TreeViewViewModel)
            },
            new PageMetadata()
            {
                Label = "金蝶测试",
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M42.5 6C43.8807 6 45 4.88071 45 3.5V2.5C45 1.11929 43.8807 0 42.5 0H13.5C12.1193 0 11 1.11929 11 2.5V3.5C11 4.88071 12.1193 6 13.5 6H42.5ZM43 3.5C43 3.77614 42.7761 4 42.5 4H13.5C13.2238 4 13 3.77614 13 3.5V2.5C13 2.22386 13.2238 2 13.5 2H42.5C42.7761 2 43 2.22386 43 2.5V3.5ZM42.5 17C43.8807 17 45 15.8807 45 14.5V13.5C45 12.1193 43.8807 11 42.5 11H18.5C17.1193 11 16 12.1193 16 13.5V14.5C16 15.8807 17.1193 17 18.5 17H42.5ZM43 14.5C43 14.7761 42.7761 15 42.5 15H18.5C18.2238 15 18 14.7761 18 14.5V13.5C18 13.2239 18.2238 13 18.5 13H42.5C42.7761 13 43 13.2239 43 13.5V14.5ZM45 27.5C45 28.8807 43.8807 30 42.5 30H18.5C17.1193 30 16 28.8807 16 27.5V26.5C16 25.1193 17.1193 24 18.5 24H42.5C43.8807 24 45 25.1193 45 26.5V27.5ZM42.5 28C42.7761 28 43 27.7761 43 27.5V26.5C43 26.2239 42.7761 26 42.5 26H18.5C18.2238 26 18 26.2239 18 26.5V27.5C18 27.7761 18.2238 28 18.5 28H42.5ZM42.5 42C43.8807 42 45 40.8807 45 39.5V38.5C45 37.1193 43.8807 36 42.5 36H13.5C12.1193 36 11 37.1193 11 38.5V39.5C11 40.8807 12.1193 42 13.5 42H42.5ZM43 39.5C43 39.7761 42.7761 40 42.5 40H13.5C13.2238 40 13 39.7761 13 39.5V38.5C13 38.2239 13.2238 38 13.5 38H42.5C42.7761 38 43 38.2239 43 38.5V39.5ZM4.83204 5.25192C4.43621 5.84566 3.56376 5.84566 3.16794 5.25192L1.03645 2.0547C0.593416 1.39015 1.06981 0.5 1.8685 0.5L6.13147 0.5C6.93016 0.5 7.40656 1.39015 6.96352 2.0547L4.83204 5.25192ZM11.7519 15.3321C12.3456 14.9362 12.3456 14.0638 11.7519 13.6679L8.55469 11.5365C7.89013 11.0934 6.99999 11.5698 6.99999 12.3685L6.99998 16.6315C6.99998 17.4302 7.89013 17.9066 8.55468 17.4635L11.7519 15.3321ZM11.7519 26.6679C12.3456 27.0638 12.3456 27.9362 11.7519 28.3321L8.55468 30.4635C7.89013 30.9066 6.99998 30.4302 6.99998 29.6315L6.99999 25.3685C6.99999 24.5698 7.89013 24.0934 8.55469 24.5365L11.7519 26.6679ZM3.16794 41.2519C3.56376 41.8457 4.43621 41.8457 4.83204 41.2519L6.96352 38.0547C7.40656 37.3901 6.93016 36.5 6.13147 36.5H1.8685C1.06981 36.5 0.593416 37.3901 1.03645 38.0547L3.16794 41.2519Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(KingdeeViewModel)
            },
            new PageMetadata()
            {
                Label = "测试模块",
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M42.5 6C43.8807 6 45 4.88071 45 3.5V2.5C45 1.11929 43.8807 0 42.5 0H13.5C12.1193 0 11 1.11929 11 2.5V3.5C11 4.88071 12.1193 6 13.5 6H42.5ZM43 3.5C43 3.77614 42.7761 4 42.5 4H13.5C13.2238 4 13 3.77614 13 3.5V2.5C13 2.22386 13.2238 2 13.5 2H42.5C42.7761 2 43 2.22386 43 2.5V3.5ZM42.5 17C43.8807 17 45 15.8807 45 14.5V13.5C45 12.1193 43.8807 11 42.5 11H18.5C17.1193 11 16 12.1193 16 13.5V14.5C16 15.8807 17.1193 17 18.5 17H42.5ZM43 14.5C43 14.7761 42.7761 15 42.5 15H18.5C18.2238 15 18 14.7761 18 14.5V13.5C18 13.2239 18.2238 13 18.5 13H42.5C42.7761 13 43 13.2239 43 13.5V14.5ZM45 27.5C45 28.8807 43.8807 30 42.5 30H18.5C17.1193 30 16 28.8807 16 27.5V26.5C16 25.1193 17.1193 24 18.5 24H42.5C43.8807 24 45 25.1193 45 26.5V27.5ZM42.5 28C42.7761 28 43 27.7761 43 27.5V26.5C43 26.2239 42.7761 26 42.5 26H18.5C18.2238 26 18 26.2239 18 26.5V27.5C18 27.7761 18.2238 28 18.5 28H42.5ZM42.5 42C43.8807 42 45 40.8807 45 39.5V38.5C45 37.1193 43.8807 36 42.5 36H13.5C12.1193 36 11 37.1193 11 38.5V39.5C11 40.8807 12.1193 42 13.5 42H42.5ZM43 39.5C43 39.7761 42.7761 40 42.5 40H13.5C13.2238 40 13 39.7761 13 39.5V38.5C13 38.2239 13.2238 38 13.5 38H42.5C42.7761 38 43 38.2239 43 38.5V39.5ZM4.83204 5.25192C4.43621 5.84566 3.56376 5.84566 3.16794 5.25192L1.03645 2.0547C0.593416 1.39015 1.06981 0.5 1.8685 0.5L6.13147 0.5C6.93016 0.5 7.40656 1.39015 6.96352 2.0547L4.83204 5.25192ZM11.7519 15.3321C12.3456 14.9362 12.3456 14.0638 11.7519 13.6679L8.55469 11.5365C7.89013 11.0934 6.99999 11.5698 6.99999 12.3685L6.99998 16.6315C6.99998 17.4302 7.89013 17.9066 8.55468 17.4635L11.7519 15.3321ZM11.7519 26.6679C12.3456 27.0638 12.3456 27.9362 11.7519 28.3321L8.55468 30.4635C7.89013 30.9066 6.99998 30.4302 6.99998 29.6315L6.99999 25.3685C6.99999 24.5698 7.89013 24.0934 8.55469 24.5365L11.7519 26.6679ZM3.16794 41.2519C3.56376 41.8457 4.43621 41.8457 4.83204 41.2519L6.96352 38.0547C7.40656 37.3901 6.93016 36.5 6.13147 36.5H1.8685C1.06981 36.5 0.593416 37.3901 1.03645 38.0547L3.16794 41.2519Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(TestViewModel)
            },
        };


        public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SetttingsIconColor = new SolidColorBrush(Colors.Black);
        }

        private void OnLoaded()
        {
            _navigationService.Navigated += OnNavigated;
        }

        private void OnUnloaded()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack() => _navigationService.CanGoBack;

        private void OnGoBack()
        {
            _navigationService.GoBack();
        }

        private void NavigateTo(Type targetViewModel)
        {
            if (targetViewModel != null)
            {
                _navigationService.NavigateTo(targetViewModel.FullName);
            }
        }

        private void OnNavigated(object sender, string viewModelName)
        {
            var item = MenuItems
                .OfType<PageMetadata>()
                .FirstOrDefault(i => viewModelName == i.TargetType?.FullName);

            if (item != null)
            {
                SelectedMenuItem = item;
            }

            GoBackCommand.OnCanExecuteChanged();
        }

        private void OnOptionsMenuItemInvoked()
        {
            NavigateTo(typeof(SettingsViewModel));
        }
    }
}
