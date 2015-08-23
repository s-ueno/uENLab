## What is uENLab?

[uEN Laboratory](http://s-ueno.github.io/) is a framework for receiving fully the benefits of MVVM technology when developing LOB.

Developers By applying the framework, you can get a MVVM and Meto-Style.


![READ ME 01](http://s-ueno.github.io/images/readme_01.PNG)


## Dependency injection container

It will provide a type-safe container using the [MEF](https://msdn.microsoft.com/library/dd460648.aspx).



## sample
```
public interface IHello
{
    void Func();
}

[PartCreationPolicy(CreationPolicy.NonShared)]
[ExportMetadata(Repository.Priority, 100)]
[Export(typeof(IHello))]
public class Hello : IHello
{
    public void Func()
    {
        Console.Write("Hello");
    }
}

[PartCreationPolicy(CreationPolicy.NonShared)]
[ExportMetadata(Repository.Priority, 10)]
[Export(typeof(IHello))]
public class PriorityHello : IHello
{
    public void Func()
    {
        Console.Write("PriorityHello");
    }
}


static void Main()
{
    //svc is PriorityHello
    var svc = Repository.GetPriorityExport<IHello>();
}
```

## app.config
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="Repository.AssemblyCatalog" type="System.Configuration.NameValueSectionHandler"/>
    <section name="Repository.TypeCatalog" type="System.Configuration.NameValueSectionHandler"/>
  </configSections>

  
  <Repository.AssemblyCatalog>
    <add key="uEN" value=""/>
    <add key="SimpleApp" value=""/>
  </Repository.AssemblyCatalog>
  <Repository.TypeCatalog>
  </Repository.TypeCatalog>
```



## The latest Visual Studio Template

Download this
<a href="http://s-ueno.github.io/additionalData/Templates.zip" rel="tooltip" title="download zip">
  <img class="social_icon" alt="icon" src="http://s-ueno.github.io/images/zippedFile.png">
</a>

Unzip the Template, you can use it to copy below.


```
C:/Users/<user>/Documents/Visual Studio XXXX/Templates
```


## License
(The MIT License)

Copyright (c) 2014-2015 uEN

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
