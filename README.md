# Zongsoft.Daemon.Launcher

![license](https://img.shields.io/github/license/Zongsoft/Zongsoft.Daemon.Launcher) ![download](https://img.shields.io/nuget/dt/Zongsoft.Daemon.Launcher) ![version](https://img.shields.io/github/v/release/Zongsoft/Zongsoft.Daemon.Launcher?include_prereleases) ![github stars](https://img.shields.io/github/stars/Zongsoft/Zongsoft.Daemon.Launcher?style=social)

README: [English](https://github.com/Zongsoft/Zongsoft.Daemon.Launcher/blob/master/README.md) | [简体中文](https://github.com/Zongsoft/Zongsoft.Daemon.Launcher/blob/master/README-zh_CN.md)

-----

这是 [**Zongsoft**](https://github.com/Zongsoft) 插件应用的后台服务宿主程序。


<a name="deploy"></a>
## 部署

### 部署文件

项目根目录的 [`.deploy`](https://github.com/Zongsoft/Zongsoft.Web.Launcher/blob/master/.deploy) 文件是一个 `INI` 格式的部署文件，它定义了需要将哪些源文件拷贝到本项目的相应目录中，供 [Zongsoft.Utilities.Deployer](https://github.com/Zongsoft/Zongsoft.Utilities.Deployer) 部署工具解析使用。

> 提示：可以参考 [Zongsoft.CoreLibrary](https://github.com/Zongsoft/Zongsoft.CoreLibrary) 核心库的 `Zongsoft.Options.Profiles` 命名空间了解 `INI` 配置文件的解析。

### 部署命令
部署命令即调用 [Zongsoft.Utilities.Deployer](https://github.com/Zongsoft/Zongsoft.Utilities.Deployer) 部署工具进行一系列文件复制。在宿主项目的“设置”-“生成事件(前)”中添加命令行 _（如下所示）_，以便每次重建 _**(Rebuild)**_ 时自动更新部署：
```bash
cd $(ProjectDir)
$(ProjectDir)\deploy-$(ConfigurationName).bat
```

> **注意：** 宿主项目本身没有多少代码，而当宿主项目没有代码变更时，在某些 **V**isual **S**tudio 版本中重新生成项目是 **不会** 激发“**生成事件**”的，因此也就没有执行在项目设置中定义的部署命令，所以这种情况下当一些插件项目编译更新后，务必手动执行一遍宿主项目根目录中的部署命令，以确保更新后的插件被正确复制过来了再运行宿主程序。

- `deploy-debug.bat` 命令文件
> ```bash
> Zongsoft.Utilities.Deployer.exe -edition:Debug ".deploy"
> ```

- `deploy-release.bat` 命令文件
> ```bash
> Zongsoft.Utilities.Deployer.exe -edition:Release ".deploy"
> ```


<a name="install"></a>
## 安装：
1. 执行 bin 目录下的 `Install.bat` 脚本文件
2. 打开「控制面板」-「管理工具」-「服务」，在其中找到你挂载并安装成功的后台服务。

<a name="uninstall"></a>
## 卸载：
1. 执行 `Uninstall.bat` 脚本文件
2. 打开「控制面板」-「管理工具」-「服务」，在其中验证指定的后台服务是否都已成功卸载。


<a name="other"></a>
## 其他

最后，希望你能喜欢 _**Zongsoft**_ 面向服务的插件架构和插件化开发方式。


<a name="contribution"></a>
## 贡献

请不要在项目的 **I**ssues 中提交询问(**Q**uestion)以及咨询讨论，**I**ssue 是用来报告问题(**B**ug)和功能特性(**F**eature)。如果你希望参与贡献，欢迎提交 代码合并请求(_[**P**ull**R**equest](https://github.com/Zongsoft/Zongsoft.Security/pulls)_) 或问题反馈(_[**I**ssue](https://github.com/Zongsoft/Zongsoft.Security/issues)_)。

对于新功能，请务必创建一个功能反馈(_[**I**ssue](https://github.com/Zongsoft/Zongsoft.Security/issues)_)来详细描述你的建议，以便我们进行充分讨论，这也将使我们更好的协调工作防止重复开发，并帮助你调整建议或需求，使之成功地被接受到项目中。

欢迎你为我们的开源项目撰写文章进行推广，如果需要我们在官网(_[http://zongsoft.com/blog](http://zongsoft.com/blog)_) 中转发你的文章、博客、视频等可通过 [**电子邮件**](mailto:zongsoft@qq.com) 联系我们。

> 强烈推荐阅读 [《提问的智慧》](https://github.com/ryanhanwu/How-To-Ask-Questions-The-Smart-Way/blob/master/README-zh_CN.md)、[《如何向开源社区提问题》](https://github.com/seajs/seajs/issues/545) 和 [《如何有效地报告 Bug》](http://www.chiark.greenend.org.uk/~sgtatham/bugs-cn.html)、[《如何向开源项目提交无法解答的问题》](https://zhuanlan.zhihu.com/p/25795393)，更好的问题更容易获得帮助。


<a name="sponsor"></a>
## 支持赞助

非常期待您的支持与赞助，可以通过下面几种方式为我们提供必要的资金支持：

1. 关注 **Zongsoft 微信公众号**，对我们的文章进行打赏；
2. 加入 [**Zongsoft 知识星球号**](https://t.zsxq.com/2nyjqrr)，可以获得在线问答和技术支持；
3. 如果您的企业需要现场技术支持与辅导，又或者需要特定新功能、即刻的错误修复等请[发邮件](mailto:zongsoft@qq.com)给我。

[![微信公号](https://raw.githubusercontent.com/Zongsoft/Guidelines/master/zongsoft-qrcode%28wechat%29.png)](http://weixin.qq.com/r/zy-g_GnEWTQmrS2b93rd)

[![知识星球](https://raw.githubusercontent.com/Zongsoft/Guidelines/master/zongsoft-qrcode%28zsxq%29.png)](https://t.zsxq.com/2nyjqrr)
