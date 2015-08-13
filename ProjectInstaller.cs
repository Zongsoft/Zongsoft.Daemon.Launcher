/*
 * Authors:
 *   钟峰(Popeye Zhong) <zongsoft@gmail.com>
 *
 * Copyright (C) 2013 Zongsoft Corporation <http://www.zongsoft.com>
 *
 * This file is part of Zongsoft.Daemon.Launcher.
 *
 * Zongsoft.Daemon.Launcher is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * Zongsoft.Daemon.Launcher is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with Zongsoft.Daemon.Launcher; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Text;

using Zongsoft.Plugins;
using Zongsoft.Services;

namespace Zongsoft.Daemon.Launcher
{
	[RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		#region 构造函数
		public ProjectInstaller()
		{
			InitializeComponent();
		}
		#endregion

		#region 初始化器
		private void InitializeComponent()
		{
			this.Installers.Add(new ServiceProcessInstaller()
			{
				Account = ServiceAccount.LocalSystem,
				Password = null,
				Username = null,
			});

			var installers = GetServiceInstallers();

			if(installers != null && installers.Length > 0)
				this.Installers.AddRange(installers);
		}
		#endregion

		#region 内部方法
		internal ServiceInstaller[] GetServiceInstallers()
		{
			string servicesPath = this.GetServicesPath();

			if(string.IsNullOrWhiteSpace(servicesPath))
				servicesPath = @"/Workbench/Startup";
			else
			{
				if(servicesPath[0] != '/')
					servicesPath = '/' + servicesPath;
			}

			//创建服务安装器列表
			List<ServiceInstaller> installers = new List<ServiceInstaller>();

			//确保插件树被加载
			ApplicationContext.Current.PluginContext.PluginTree.Load();

			//获取服务挂载的根节点
			var node = ApplicationContext.Current.PluginContext.PluginTree.Find(servicesPath);

			if(node == null)
			{
				string message = Zongsoft.Resources.ResourceUtility.GetString("Text.InstallFailed.Message", this.GetType().Assembly, ApplicationContext.Current.PluginContext.Settings.PluginsDirectory, servicesPath);
				this.WriteLog(message);
			}
			else
			{
				//从挂载在启动路径下的工作者并将其装包在服务包装器中
				this.GetServiceInstallers(installers, node);
			}

			return installers.ToArray();
		}
		#endregion

		#region 私有方法
		private void WriteLog(string message)
		{
			if(string.IsNullOrEmpty(message))
				return;

			var directoryPath = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
			var filePath = System.IO.Path.Combine(directoryPath, "Install.log");

			using(var file = new System.IO.FileStream(filePath, System.IO.FileMode.Append, System.IO.FileAccess.Write))
			{
				var data = Encoding.UTF8.GetBytes(message + Environment.NewLine);
				file.Write(data, 0, data.Length);
			}
		}

		private string GetServicesPath()
		{
			var args = Environment.GetCommandLineArgs();

			foreach(string arg in args)
			{
				if(arg.Length > 0 && (arg[0] == '/' || arg[0] == '-'))
				{
					var parts = arg.Split('=');

					if(parts.Length == 0)
						parts = arg.Split(':');

					if(parts.Length == 2)
					{
						string name = parts[0].Substring(1);

						if(string.Equals(name, "ServicesPath", StringComparison.OrdinalIgnoreCase) ||
						   string.Equals(name, "Path", StringComparison.OrdinalIgnoreCase))
						{
							return parts[1];
						}
					}
				}
			}

			return string.Empty;
		}

		private void GetServiceInstallers(IList<ServiceInstaller> installers, PluginTreeNode node)
		{
			if(installers == null || node == null)
				return;

			if(node.NodeType != PluginTreeNodeType.Empty)
			{
				Type workerType = null;
				bool disabled = false;

				if(node.NodeType == PluginTreeNodeType.Custom)
				{
					var worker = node.UnwrapValue<Zongsoft.Services.IWorker>(ObtainMode.Auto, this, null);

					if(worker != null)
					{
						workerType = worker.GetType();
						disabled = worker.Disabled;
					}
				}
				else
				{
					if(typeof(IWorker).IsAssignableFrom(node.ValueType))
						workerType = node.ValueType;

					disabled = ((Builtin)node.Value).Properties.GetValue<bool>("disabled", false);
				}

				if(workerType != null)
				{
					ServiceInstaller installer = new ServiceInstaller();

					installer.DelayedAutoStart = false;
					installer.ServiceName = node.Name;
					installer.StartType = (disabled ? ServiceStartMode.Disabled : ServiceStartMode.Automatic);

					//设置安装服务的描述文本
					var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(workerType, typeof(DescriptionAttribute));
					if(descriptionAttribute != null)
						installer.Description = Resources.ResourceUtility.GetString(descriptionAttribute.Description, workerType.Assembly);

					//设置安装服务的显示名称
					var displayAttribute = (DisplayNameAttribute)Attribute.GetCustomAttribute(workerType, typeof(DisplayNameAttribute));
					if(displayAttribute != null)
						installer.DisplayName = Resources.ResourceUtility.GetString(displayAttribute.DisplayName, workerType.Assembly);

					installers.Add(installer);
				}
			}

			foreach(PluginTreeNode child in node.Children)
				this.GetServiceInstallers(installers, child);
		}
		#endregion
	}
}
