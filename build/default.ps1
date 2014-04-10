properties {
	$solution_name = "GoodlyFere.Criteria"
	$package_dlls = @{
		"net45" = "$solution_name\bin\Release\$solution_name.dll"
		"net35" = "$solution_name.NET35\bin\Release\$solution_name.NET35.dll"
	}
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

Framework "4.0"

task default -depends Package

task Clean {
	# remove build/package folders
	if (Test-Path $output_dir) { ri -force -recurse $output_dir }
	if (Test-Path $package_dir) { ri -force -recurse $package_dir }
	
	# clean project builds
	$projFiles = @(gci $src_dir "*.csproj" -Recurse)
	foreach ($pf in @($projFiles)) {
		exec { msbuild $pf "/t:Clean" } "msbuild clean failed."
	}
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
	cp "$nuspec_file" "$package_dir"
	
	mkdir "$package_dir\lib"
	foreach ($k in $package_dlls.Keys) {
		$path = "$src_dir\" + $package_dlls.Item($k)
		cp "$path" "$package_dir\lib"
	}
	
	$spec = [xml](get-content "$package_dir\$nuspec_file")
	$spec.package.metadata.version = ([string]$spec.package.metadata.version).replace("{Version}", $build_version)
	$spec.Save("$package_dir\$nuspec_file")
	
	exec { nuget pack "$package_dir\$nuspec_file" }
}
