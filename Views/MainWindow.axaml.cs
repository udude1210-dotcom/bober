using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Interactivity;
using ImageGalleryApp.Models;
using ImageGalleryApp.Services;

namespace ImageGalleryApp.Views;


public partial class MainWindow : Window
{
    private readonly ImageService imageService = new();

    private List<ImageInfo> images = new();

    private int currentIndex = 0;

    private Bitmap? currentBitmap;

    public MainWindow()
    {
        InitializeComponent();
    }

    // Завантаження зображень
    private void Load_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            string folders = FolderTextBox.Text ?? "";

            images = imageService.LoadImages(folders);

            currentIndex = 0;

            if (images.Count == 0)
            {
                PreviewImage.Source = null;
                ImageInfoText.Text = "Зображень не знайдено.";
                StatusText.Text = "Список порожній.";
                DisposeBitmap();
                return;
            }

            StatusText.Text =
                $"Знайдено зображень: {images.Count}";

            ShowImage();
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Помилка: {ex.Message}";
        }
    }

    // Відображення поточного зображення
    private void ShowImage()
    {
        if (images.Count == 0)
            return;

        var image = images[currentIndex];

        try
        {
            var bitmap = new Bitmap(image.FilePath);

            DisposeBitmap();

            currentBitmap = bitmap;
            PreviewImage.Source = currentBitmap;

            ImageInfoText.Text =
                $"Назва: {image.FileName}\n" +
                $"Дата створення: {image.CreationDate}\n" +
                $"Розмір файлу: {image.FileSize / 1024.0:F2} KB\n" +
                $"Роздільна здатність: " +
                $"{bitmap.PixelSize.Width} x {bitmap.PixelSize.Height}\n" +
                $"Формат: {image.Format}\n" +
                $"Зображення: {currentIndex + 1} з {images.Count}";
        }
        catch (Exception ex)
        {
            DisposeBitmap();
            PreviewImage.Source = null;

            ImageInfoText.Text =
                $"Не вдалося відкрити файл: {image.FileName}\n{ex.Message}";
        }
    }

    // Перехід до попереднього зображення
    private void Previous_Click(
        object? sender,
        Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (images.Count == 0)
            return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = images.Count - 1;

        ShowImage();
    }

    // Перехід до наступного зображення
    private void Next_Click(
        object? sender,
        Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (images.Count == 0)
            return;

        currentIndex++;

        if (currentIndex >= images.Count)
            currentIndex = 0;

        ShowImage();
    }

    // Закриття програми
    private void CloseProgram_Click(
        object? sender,
        Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void DisposeBitmap()
    {
        PreviewImage.Source = null;

        currentBitmap?.Dispose();
        currentBitmap = null;
    }

    protected override void OnClosed(EventArgs e)
    {
        DisposeBitmap();
        base.OnClosed(e);
    }
}