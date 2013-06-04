# Runs every time a package is installed in a project

param($installPath, $toolsPath, $package, $project)

# $installPath is the path to the folder where the package is installed.
# $toolsPath is the path to the tools directory in the folder where the package is installed.
# $package is a reference to the package object.
# $project is a reference to the project the package was installed to.


$item = $project.ProjectItems | where-object {$_.Name -eq "defaultAdSettings.xml"}

$item.Properties.Item("BuildAction").Value = [int]2
$item.Properties.Item("CopyToOutputDirectory").Value = [int]1

$item2 = $project.ProjectItems | where-object {$_.Name -eq "ReadMe-AdRotator-XAML.txt"}

$item2.Properties.Item("BuildAction").Value = [int]0
$item2.Properties.Item("CopyToOutputDirectory").Value = [int]0