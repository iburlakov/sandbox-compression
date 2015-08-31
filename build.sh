rm -r sandbox-compression/bin/Debug
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool build sandbox-compression.sln

cd sandbox-compression/bin/Debug
mkdir embedded
cd embedded

export AS="as -arch i386"
export CC="clang -arch i386 -mmacosx-version-min=10.6"
mkbundle -o sandbox-compression --machine-config /Library/Frameworks/Mono.framework/Versions/3.2.4/etc/mono/2.0/machine.config --nodeps -z ../sandbox-compression.exe ../../../ext/ICSharpCode.SharpZipLib.dll System.dll System.Configuration.dll System.Security.dll /Library/Frameworks/Mono.framework/Versions/3.2.4/lib/mono/4.5/mscorlib.dll

rm -R sandbox-compression.dSYM

cp /Library/Frameworks/Mono.framework/Versions/3.2.4/lib/libmono-2.0.1.dylib .
cp /Library/Frameworks/Mono.framework/Versions/3.2.4/lib/libmonoboehm-2.0.1.dylib .

install_name_tool -change /Library/Frameworks/Mono.framework/Versions/3.2.4/lib/libmono-2.0.1.dylib @executable_path/libmono-2.0.1.dylib sandbox-compression
install_name_tool -change /Library/Frameworks/Mono.framework/Versions/3.2.4/lib/libmonoboehm-2.0.1.dylib @executable_path/libmonoboehm-2.0.1.dylib sandbox-compression

cd ../../../../