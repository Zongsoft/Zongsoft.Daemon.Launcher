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
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Zongsoft.Plugins;

namespace Zongsoft.Daemon.Launcher
{
	public class ApplicationContext : Zongsoft.Plugins.PluginApplicationContext
	{
		#region 静态字段
		public static readonly ApplicationContext Current = new ApplicationContext();
		#endregion

		#region 成员字段
		private Zongsoft.Options.Configuration.OptionConfiguration _configuration;
		#endregion

		#region 私有构造
		private ApplicationContext() : base("Zongsoft.Daemon.Launcher")
		{
			Zongsoft.ComponentModel.ApplicationContextBase.Current = this;
		}
		#endregion

		#region 重写属性
		/// <summary>
		/// 获取当前应用程序的根目录完整路径。
		/// </summary>
		/// <remarks>
		///		<para>注意：在基类默认实现中，插件环境的应用目录为当前应用域(AppDomain)的基目录(即 AppDomain.CurrentDomain.BaseDirectory)，这种实现方式在控制台、富客户端以及Web应用程序中均能很好工作，但是在Windows系统服务(Windows-Service)程序中，应用域(AppDomain)的基目录表示的是Installutil.exe安装程序所在目录，而这将导致插件加载器被定位到错误的插件目录。</para>
		/// </remarks>
		public override string ApplicationDirectory
		{
			get
			{
				return Path.GetDirectoryName(this.GetType().Assembly.Location);
			}
		}

		public override Zongsoft.Options.Configuration.OptionConfiguration Configuration
		{
			get
			{
				if(_configuration == null)
				{
					string filePaht = Path.Combine(this.ApplicationDirectory, Assembly.GetEntryAssembly().GetName().Name) + ".option";

					if(File.Exists(filePaht))
						_configuration = Options.Configuration.OptionConfiguration.Load(filePaht);
					else
						_configuration = new Options.Configuration.OptionConfiguration(filePaht);
				}

				return _configuration;
			}
		}
		#endregion

		#region 重写方法
		protected override IWorkbenchBase CreateWorkbench(string[] args)
		{
			PluginTreeNode node = this.PluginContext.PluginTree.Find(this.PluginContext.Settings.WorkbenchPath);

			if(node != null && node.NodeType == PluginTreeNodeType.Builtin)
				return base.CreateWorkbench(args);

			return new Workbench(this);
		}
		#endregion
	}
}
