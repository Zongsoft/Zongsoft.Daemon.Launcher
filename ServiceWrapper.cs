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
using System.ComponentModel;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Text;

using Zongsoft.Plugins;
using Zongsoft.Services;

namespace Zongsoft.Daemon.Launcher
{
	public class ServiceWrapper : ServiceBase
	{
		#region 成员字段
		private IWorker _worker;
		#endregion

		#region 构造函数
		public ServiceWrapper(IWorker worker)
		{
			if(worker == null)
				throw new ArgumentNullException("worker");

			_worker = worker;

			this.ServiceName = worker.Name;
			this.CanPauseAndContinue = worker.CanPauseAndContinue;
		}
		#endregion

		#region 重写方法
		protected override void OnStart(string[] args)
		{
			_worker.Start(args);
		}

		protected override void OnStop()
		{
			_worker.Stop();
		}

		protected override void OnPause()
		{
			_worker.Pause();
		}

		protected override void OnContinue()
		{
			_worker.Resume();
		}
		#endregion
	}
}
