require 'albacore'
require 'date'
require 'fileutils'
require 'version_bumper'

@solution_file = "CoderMike.EasySettings.sln"
@project_name = "CoderMike.EasySettings"
@default_configuration = :Release
@configurations = [:Debug, :Release]
@generated_files = ["SolutionAssemblyInfo.cs"]
@cache_directories = ["packages", "output"]
@owners = "Mike Minutillo"
@authors = "CoderMike, Colin David Scott"
@nuget_path = ".nuget\\NuGet.exe"
@copyright_years = "2011-2013"

task :default => [:clean, :version, :installNuGetPackages, :build, :test, :package]

desc "Clean All The Things"
task :clean do
  @configurations.each do |configuration|
    Rake::Task["clean_build"].execute({ :configuration => configuration })
  end
  @generated_files.each do |file|
    next if !File.exists?(file)
    puts "Removing generated file '#{file}'"
    File.delete(file)
  end
  @cache_directories.each do |dir|
    FileUtils.rm_rf dir
  end
end

desc "Clean a particular build"
msbuild :clean_build, :configuration do |msb, args|
  puts "Cleaning #{args[:configuration]} Configuration"
  
  msb.properties = { :configuration => args[:configuration] }
  msb.targets = [ :Clean ]
  msb.solution = @solution_file
end

desc "Generate the shared version file"
assemblyinfo :version do |asm|
  asm.version = bumper_version.to_s
  asm.file_version = generate_file_version
  asm.company_name = @owners
  asm.product_name = "#{@project_name} (Scripted)"
  asm.copyright = "Copyright #{@authors} #{@copyright_years}"
  asm.custom_attributes :AssemblyConfiguration => @default_configuration.to_s, :AssemblyInformationalVersion => bumper_version.to_s
  asm.output_file = "SolutionAssemblyInfo.cs"
end

def generate_file_version
  current_time = Time.now.utc
  current_date = Time.utc(current_time.year, current_time.month, current_time.day)
  
  build = (current_time - Time.utc(2012,1,1)).to_i / (60 * 60 * 24)
  revision = ((current_time - current_date) * 0.7584).to_i

  "#{bumper_version.major}.#{bumper_version.minor}.#{build}.#{revision}"
end

task :installNuGetPackages do
  nuget_path = @nuget_path
  FileList["**/packages.config", ".nuget/packages.config"].each { |filepath|
    sh "#{nuget_path} install \"#{filepath}\" -OutputDirectory packages"
  }
end

desc "Build #{@project_name}"
msbuild :build do |msb|
  msb.properties = { :configuration => @default_configuration }
  msb.targets = [ :Build ]
  msb.solution = @solution_file
end

desc "Run the tests"
xunit :test do |xunit|
  files = FileList.new("**/#{@default_configuration.to_s}/**/*.Tests.dll")
    .exclude('packages/**/*')
    .exclude('**/obj/**/*')
    .map {|file| '"' + file + '"' }
    .to_a

  xunit.command = FileList["packages/**/xunit.console.clr4.x86.exe"][0]
  xunit.assemblies files
end

task :package do
  if !Dir.exist?("output")
    Dir.mkdir("output")
  end

  sh "#{@nuget_path} pack CoderMike.EasySettings\\CoderMike.EasySettings.csproj -Prop Configuration=Release -OutputDirectory output"
  sh "#{@nuget_path} pack CoderMike.Autofac.EasySettings\\CoderMike.Autofac.EasySettings.csproj -Prop Configuration=Release -OutputDirectory output"
end