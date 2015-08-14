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

namespace Zongsoft.Daemon.Launcher
{
	internal static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		internal static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
			Zongsoft.Plugins.Application.Start(ApplicationContext.Current, args);
		}

		private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var exception = e.ExceptionObject as Exception;
			var level = e.IsTerminating ? Zongsoft.Diagnostics.LogLevel.Fatal : Zongsoft.Diagnostics.LogLevel.Error;

			if(exception == null)
				Zongsoft.Diagnostics.Logger.Log(level, "UnhandledException", e.ExceptionObject);
			else
				Zongsoft.Diagnostics.Logger.Log(level, exception);
		}
	}
}