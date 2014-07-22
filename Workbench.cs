﻿/*
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
using System.Collections.Generic;
using System.ServiceProcess;
using System.Linq;
using System.Text;

using Zongsoft.Plugins;
using Zongsoft.Services;

namespace Zongsoft.Daemon.Launcher
{
	public class Workbench : WorkbenchBase
	{
		#region 构造函数
		internal Workbench(PluginApplicationContext applicationContext) : base(applicationContext)
		{
		}
		#endregion

		#region 重写方法
		protected override void OnStart(string[] args)
		{
			var services = this.GetServices();

			if(services.Length > 0)
				ServiceBase.Run(services);
		}
		#endregion

		#region 内部方法
		internal ServiceBase[] GetServices()
		{
			//创建服务列表
			List<ServiceBase> services = new List<ServiceBase>();

			//从挂载在启动路径下的工作者并将其装包在服务包装器中
			this.GetServices(services, this.PluginContext.PluginTree.Find(this.StartupPath));

			return services.ToArray();
		}
		#endregion

		#region 私有方法
		private void GetServices(IList<ServiceBase> services, PluginTreeNode node)
		{
			if(services == null || node == null)
				return;

			if(node.NodeType != PluginTreeNodeType.Empty)
			{
				object value = node.UnwrapValue(ObtainMode.Auto, this, null);

				if(value is ServiceBase)
					services.Add((ServiceBase)value);
				else if(value is IWorker)
					services.Add(new ServiceWrapper((IWorker)value));
			}

			foreach(PluginTreeNode child in node.Children)
				this.GetServices(services, child);
		}
		#endregion
	}
}