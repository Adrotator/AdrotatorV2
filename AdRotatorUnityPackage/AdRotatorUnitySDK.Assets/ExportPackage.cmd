@echo off
cls
echo Exporting Ad Rotator Unity SDK
"%~2" -quit -batchmode -nographics -executeMethod "AdRotatorUnitySDK.Assets.PackageRelease.ExportSdk" -projectPath "%~1AdRotatorUnitySDK\Demo"
echo Exporting Ad Rotator Unity SDK - DEMO
"%~2" -quit -batchmode -nographics -executeMethod "AdRotatorUnitySDK.Assets.PackageRelease.ExportDemo" -projectPath "%~1AdRotatorUnitySDK\Demo"
