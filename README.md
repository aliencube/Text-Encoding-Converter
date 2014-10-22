# Text Encoding Converter #

**Text Encoding Converter** converts text files encoding from one to another, mainly `ks_c_5601-1987` to `UTF-8`.


## Getting Started ##

### Installation ###

There is no need for installation. Instead, download one of the following options:

* [Console Application](http://github.aliencube.org/Text-Encoding-Converter/downloads/TextEncodingConverter-Console-2.0.0.0.zip)
* [Windows GUI Application](http://github.aliencube.org/Text-Encoding-Converter/downloads/TextEncodingConverter-Windows-2.0.0.0.zip)


### Configuration ###

Once unzip the file, it works with its default settings. However, if you want to change some settings, you should look after the `Aliencube.TextEncodingConverter.exe.config` file.

```xml
<textEncodingConverterSettings>
    <encoding input="ks_c_5601-1987" output="utf-8" />
    <converter extensions="csv,txt" backup="true" backupPath="Backup" outputPath="Output" />
</textEncodingConverterSettings>
```

* `encoding.input`: Input encoding format. Default value is `ks_c_5601-1987`.
* `encoding.output`: Output encoding format. Default value is `utf-8`.
* `converter.extensions`: Comma delimited extensions to convert. As a default, the application only consider `.csv` and `.txt` files.
* `converter.backup`: Value that specifies whether to backup the original files or not. Default value is `true`.
* `converter.backupPath`: Relative or absolute path for backup. Default value is `Backup`. If `converter.backup` is set to `false`, this will be ignored.
* `converter.outputPath`: Relative or absolute path for output. Default value is `Output`.


### Console Application ###

**Text Encoding Converter** console application requires the following parameters:

* `/d`|`/f`: Indicates whether the input is directory or file.
* `/ie:xxxx`: specifies the input file encoding. It can be either codepage or IANA name defined [here](http://msdn.microsoft.com/en-us/library/System.Text.Encoding(v=vs.110).aspx). Double quote might be necessary for input directory/file path.
* `/oe:xxxx`: specifies the output file encoding. It can be either codepage or IANA name defined [here](http://msdn.microsoft.com/en-us/library/System.Text.Encoding(v=vs.110).aspx). Double quote might be necessary for output directory/file path.
* `/i:xxxx`: specifies the input path. It can be either directory or file.
* `/o:xxxx`: specifies the output directory path.

Here is a sample command:

```
Aliencube.TextEncodingConverter.ConsoleApp.exe /d /ie:949 /oe:utf-8 /i:sample /o:output
```


### Windows GUI Application ###

**Text Encoding Converter** Windows GUI application is intuitive to use:

* Choose input encoding option: `ks_c_5601-1987` is initially set as a default.
* Choose output encoding option: `utf-8` is initially set as a default.
* Select files to convert: Click the `Browse` button to select files.
* Convert: Click the `Convert` button to convert.


## Future Release ##

* Console Application for Mac
* GUI Application for Mac


## Contribution ##

Your contributions are always welcome! All your work should be done in your forked repository. Once you finish your work, please send us a pull request onto our `dev` branch for review.


## License ##

**Text Encoding Converter** is released under [MIT License](http://opensource.org/licenses/MIT)

> The MIT License (MIT)
>
> Copyright (c) 2014 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
> 