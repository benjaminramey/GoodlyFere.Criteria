properties {
  $framework = "v4.5"
  $solution_name = "GoodlyFere.Criteria"
  $base_dir  = resolve-path ..\
  $build_base_dir = "$base_dir\build"
  $output_dir = "$build_base_dir\output"
  $packageinfo_dir = "$base_dir"
  $debug_build_dir = "$output_dir\bin\debug"
  $release_build_dir = "$output_dir\bin\release"
  $release_dir = "$build_base_dir\Release"
  $sln_file = "$base_dir\src\$solution_name.sln"
  $tools_dir = "$build_base_dir\Tools"
  $run_tests = $true
  $xunit_console = "$tools_dir\xunit.console.clr4.exe"
}

Framework "4.0"

task default -depends Package

task Clean {
  if (Test-Path $output_dir) { remove-item -force -recurse $output_dir }
  if (Test-Path $release_dir) { remove-item -force -recurse $release_dir }
}

task Init -depends Clean {
	mkdir @($release_dir, $output_dir) | out-null
	
    #UpdateVersion
}

task Compile -depends Init {
  Exec { msbuild $sln_file /p:"OutDir=$debug_build_dir\;Configuration=Debug;TargetFrameworkVersion=$framework" } "msbuild (debug) failed."
  Exec { msbuild $sln_file /p:"OutDir=$release_build_dir\;Configuration=Release;TargetFrameworkVersion=$framework" } "msbuild (release) failed."
}

task Test -depends Compile -precondition { return $run_tests }{
  cd $debug_build_dir
  Exec { & $xunit_console "$solution_name.Tests.dll" } "xunit failed."
}

task Package -depends Compile, Test {
  $spec_files = @(Get-ChildItem $packageinfo_dir "*.nuspec" -Recurse)

  foreach ($spec in @($spec_files))
  {
	$dir =  $($spec.Directory)
	cd $dir
    Exec { nuget pack -o $release_dir -Properties Configuration=Release`;OutDir=$release_build_dir\ -Symbols } "nuget pack failed."
  }
}
