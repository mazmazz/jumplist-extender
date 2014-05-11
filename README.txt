Jumplist Extender is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Jumplist Extender is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

========================


INTRO

Thanks for downloading the Jumplist Extender (JLE) v0.3 source! JLE is written primarily in C#, and its contents consist of:

/T7EBackground - The background process that assigns windows to their jumplists

/T7EPreferences - The config app that does all the work of setting up jumplists

/Libraries - External code library sources, used with T7EBackground and T7EPreferences
----/Windows API Code Pack - Windows 7 integration library for C#. Implements jumplists for .NET 3.5.
----/IconLib - A Vista-compatible icon library, useful for outputting various formats.
----/ImageComboBox - A ComboBox winforms control that displays icons with its entries. Compiled, but currently unused.

/Common - Miscellaneous resources that are used by both T7EBackground and T7EPreferences. They consist of program icons at first, but when you compile the main solution, the library DLLs will be placed here for T7EBackground and T7EPreferences to use.

/Extras - Extra standalone code snippets, e.g. beta jumplist code or sandbox tests. These are not compiled with the other projects.

The main solution will build into /bin, with everything you need to run the complete program.

To build an installation file, first install InnoSetup 5 or later to the default location (`%ProgramFiles(x86)%\Inno Setup 5\`). Then build NSISInstaller project /in the Release configuration/ to activate the post-build actions that produce the install. Result installation file will be created in `\NSISInstaller\Output\` folder.


LICENSE

Unless otherwise specified, the pieces of Jumplist Extender are released under the GNU GPLv3.

AutoHotKey is free and open source software written by Chris Mallett, released under the GNU GPLv2.

Windows API Code Pack is written by Microsoft, and is covered under its own proprietary MICROSOFT WINDOWS API CODE PACK FOR MICROSOFT .NET FRAMEWORK license, viewable in the project's directory.

IconLib is written by Castor Tiu, presented by CodeProject.com, and is released under a Creative Commons Attribution-Share Alike 3.0 Unported License, viewable in the project's directory.

ImageComboBox is written by "sidhumn," presented by CodeProject.com, and is released under the Common Development and Distribution License Version 1.0, viewable in the project's directory.


AUTOHOTKEY SOURCE

Jumplist Extender is packaged with a binary executable of AutoHotKey, a free and open-source scripting system by Chris Mallett, for automated tasks.

You can download the C++ source at http://www.autohotkey.com/download/


WARNING

Jumplist Extender v0.3's source is VERY, VERY messy! Explore at your own peril!

Code cleanup is on the roadmap for v2.0.


CONTACT

If you have any questions or concerns, you may email me, Marco Zafra, at <digimarco35@yahoo.com>.