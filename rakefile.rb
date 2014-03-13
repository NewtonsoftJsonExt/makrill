
require 'albacore'
def nunit_cmd()
  return Dir.glob(File.join(File.dirname(__FILE__),"src","packages","NUnit.Runners.*","tools","nunit-console.exe")).first
end

namespace :ms do
  dir = File.dirname(__FILE__)
  desc "build using msbuild"
  msbuild :build do |msb|
    msb.properties :configuration => :Debug
    msb.targets :Clean, :Rebuild
    msb.verbosity = 'quiet'
    msb.solution =File.join(dir,"src", "Makrill.sln")
  end
  desc "test using nunit console"
  nunit :test => :build do |nunit|
    nunit.command = nunit_cmd()
    nunit.assemblies File.join(dir,"src","Makrill.Tests/bin/Debug/Makrill.Tests.dll")
  end

  task :core_copy_to_nuspec => [:build] do
    output_directory_lib = File.join(dir,"nuget/Makrill/lib/40/")
    mkdir_p output_directory_lib
    cp Dir.glob("./src/Makrill/bin/Debug/Makrill.dll"), output_directory_lib
  end

  desc "create the nuget package"
  task :nugetpack => [:core_nugetpack]

  task :core_nugetpack => [:core_copy_to_nuspec] do |nuget|
    cd File.join(dir,"nuget/Makrill") do
      sh "..\\..\\src\\.nuget\\NuGet.exe pack Makrill.nuspec"
    end
  end
end
namespace :mono do
   xbuild :build do |msb|
    msb.properties with_properties({:configuration => :Debug})
    msb.targets :rebuild
    msb.verbosity = 'quiet'
    msb.solution = File.join('.', "src", "Makrill.sln")
  end

  def with_properties hash
      solution_dir = File.join(File.dirname(__FILE__),'src')
      nuget_tools_path = File.join(solution_dir, '.nuget')

      to_add = {:SolutionDir => solution_dir,
      :NuGetToolsPath => nuget_tools_path,
      :NuGetExePath => File.join(nuget_tools_path, 'NuGet.exe'),
      :PackagesDir => File.join(solution_dir, 'packages')}.merge(hash)
      return to_add
  end

  desc "test with nunit"
  task :test => :build do |n|
    tlib = "Makrill.Tests"
    sh "mono --runtime=v4.0.30319 #{nunit_cmd()} src/#{tlib}/bin/Debug/#{tlib}.dll -noxml -process=single" do  |ok, res|
      if !ok
        abort 'Nunit failed!'
      end
    end
  end

  desc "Install missing NuGet packages."
  task :install_packages do |cmd|
    package_paths = FileList["src/**/packages.config"]+["src/.nuget/packages.config"]
    package_paths.each do |filepath|
      sh "mono --runtime=v4.0.30319 ./src/.nuget/NuGet.exe i #{filepath} -o ./src/packages -source http://www.nuget.org/api/v2/"
    end
  end

end