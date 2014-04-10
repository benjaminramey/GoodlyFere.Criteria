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
	$src_dir = "$base_dir\src"
	$build_dir = "$base_dir\build"
	$package_dir = "$build_dir\package"  
	$tools_dir = "$build_dir\Tools"
  
	$sln_file = "$src_dir\$solution_name.sln"
	$nuspec_file = "$src_dir\$solution_name.nuspec"
	$xunit_console = "$tools_dir\xunit.console.clr4.exe"
}

Framework "4.0"

task default -depends Package

task Clean -depends Init {
	# remove build/package folders
	if (Test-Path $package_dir) { ri -force -recurse $package_dir }
	
	# clean project builds
	$projFiles = @(gci $src_dir "*.csproj" -Recurse)
	foreach ($pf in @($projFiles)) {
		cd $pf.Directory
		exec { msbuild $pf "/t:Clean" } "msbuild clean failed."
	}
	
	cd $base_dir
	# recreate build/package folders
	mkdir @($package_dir) | out-null
}

task Init {
	cls
}

task Compile -depends Clean {
	exec { msbuild $sln_file "/p:Configuration=Release" } "msbuild (release) failed."
}

task Test -depends Compile -precondition { return $run_tests } {
	$test_dlls = @(gci $src_dir "*.Tests.dll" -Recurse)
	
	foreach ($dll in @($test_dlls)) {
		cd $dll.Directory
		exec { & $xunit_console "$dll" } "xunit failed."
	}
	
	cd $base_dir
}

task Package -depends Compile, Test {
	cp "$nuspec_file" "$package_dir"
	
	mkdir "$package_dir\lib"
	foreach ($k in $package_dlls.Keys) {
		cd $k.Directory
		$path = "$src_dir\" + $package_dlls.Item($k)
		cp "$path" "$package_dir\lib"
	}
	
	cd $base_dir
	
	$spec = [xml](get-content "$package_dir\$nuspec_file")
	$spec.package.metadata.version = ([string]$spec.package.metadata.version).replace("{Version}", $build_version)
	$spec.Save("$package_dir\$nuspec_file")
	
	exec { nuget pack "$package_dir\$nuspec_file" }
}
