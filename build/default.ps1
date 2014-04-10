properties {
  $solution_name = "GoodlyFere.Criteria"
  $version = "1.0.0.0"
  $build_version = "$version-" + (git describe --tags --long).split('-')[1]
  $run_tests = $true
  
  $base_dir  = resolve-path ..\
  $build_dir = "$base_dir\build"
  $output_dir = "$build_dir\output"
  $package_dir = "$build_dir\package"  
  $tools_dir = "$build_dir\Tools"
  $src_dir = "$build_dir\src"
  
  $sln_file = "$src_dir\$solution_name.sln"
  $xunit_console = "$tools_dir\xunit.console.clr4.exe"
}

Framework "4.5"

task default -depends Package

task Clean {
	# remove build/package folders
	if (Test-Path $output_dir) { ri -force -recurse $output_dir }
	if (Test-Path $package_dir) { ri -force -recurse $package_dir }
	
	# clean project builds
	exec { msbuild $sln_file "/t:Clean" } "msbuild clean failed."
}

task Init -depends Clean {
	cls
	
	# recreate build/package folders
	mkdir @($package_dir, $output_dir) | out-null
	
    #UpdateVersion
}

task Compile -depends Init {
	exec { msbuild $sln_file "/p:Configuration=Release" } "msbuild (release) failed."
}

task Test -depends Compile -precondition { return $run_tests } {
	$test_dlls = @(gci $src_dir "*.Tests.dll" -Recurse)
	
	foreach ($dll in @($test_dlls)) {
		exec { & $xunit_console "$dll" } "xunit failed."
	}
}

task Package -depends Compile, Test {
	cp "$nuget_spec_file" "$package_dir"
	
	mkdir "$package_dir\lib"

  $spec_files = @(gci $packageinfo_dir "*.nuspec" -Recurse)

  foreach ($spec in @($spec_files)) {
	$dir =  $($spec.Directory)
	cd $dir
    Exec { nuget pack -o $release_dir -Prop Configuration=Release -Symbols } "nuget pack failed."
  }
}
