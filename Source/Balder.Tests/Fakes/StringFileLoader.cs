#region License

//
// Author: Einar Ingebrigtsen <einar@dolittle.com>
// Copyright (c) 2007-2010, DoLittle Studios
//
// Licensed under the Microsoft Permissive License (Ms-PL), Version 1.1 (the "License")
// you may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://balder.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

using System;
using System.IO;
using Balder.Content;

namespace Balder.Tests.Fakes
{
	public class StringFileLoader : IFileLoader
	{
		public string StringToRead { get; set; }

		public Stream GetStream(string fileName)
		{
			var bytes = System.Text.Encoding.UTF8.GetBytes(StringToRead);
			var stream = new MemoryStream(bytes);
			return stream;
		}

		public bool Exists(string fileName)
		{
			throw new NotImplementedException();
		}

		public string[] SupportedSchemes
		{
			get { throw new NotImplementedException(); }
		}
	}
}
