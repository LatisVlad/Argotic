﻿/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
01/23/2008	brian.kuhn	Created WellFormedWebCommentsSyndicationExtension Class
****************************************************************************/
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Argotic.Common;

namespace Argotic.Extensions.Core
{
    /// <summary>
    /// Extends syndication specifications to provide a means exposing comments made against feed content.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <see cref="WellFormedWebCommentsSyndicationExtension"/> extends syndicated content to specify the human and machine readable endpoints for users to provide commentary on published content. 
    ///         This syndication extension conforms to the <b>Well-Formed Web Comment API</b> 1.0 specification, which can be found 
    ///         at <a href="http://wellformedweb.org/news/wfw_namespace_elements/">http://wellformedweb.org/news/wfw_namespace_elements/</a>.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code lang="cs" title="The following code example demonstrates the usage of the WellFormedWebCommentsSyndicationExtension class.">
    ///         <code 
    ///             source="..\..\Documentation\Microsoft .NET 3.5\CodeExamplesLibrary\Extensions\Core\WellFormedWebCommentsSyndicationExtensionExample.cs" 
    ///             region="WellFormedWebCommentsSyndicationExtension"
    ///         />
    ///     </code>
    /// </example>
    [Serializable()]
    public class WellFormedWebCommentsSyndicationExtension : SyndicationExtension, IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold specific information about the extension.
        /// </summary>
        private WellFormedWebCommentsSyndicationExtensionContext extensionContext = new WellFormedWebCommentsSyndicationExtensionContext();
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region WellFormedWebCommentsSyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="WellFormedWebCommentsSyndicationExtension"/> class.
        /// </summary>
        public WellFormedWebCommentsSyndicationExtension()
            : base("wfw", "http://wellformedweb.org/CommentAPI/", new Version("1.0"), new Uri("http://wellformedweb.org/news/wfw_namespace_elements/"), "Well-Formed Web Comments", "Extends syndication feeds to provide a means exposing comments made against feed content.")
        {
            //------------------------------------------------------------
            //	Initialization handled by base class
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Context
        /// <summary>
        /// Gets or sets the <see cref="WellFormedWebCommentsSyndicationExtensionContext"/> object associated with this extension.
        /// </summary>
        /// <value>A <see cref="WellFormedWebCommentsSyndicationExtensionContext"/> object that contains information associated with the current syndication extension.</value>
        /// <remarks>
        ///     The <b>Context</b> encapsulates all of the syndication extension information that can be retrieved or written to an extended syndication entity. 
        ///     Its purpose is to prevent property naming collisions between the base <see cref="SyndicationExtension"/> class and any custom properties that 
        ///     are defined for the custom syndication extension.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public WellFormedWebCommentsSyndicationExtensionContext Context
        {
            get
            {
                return extensionContext;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                extensionContext = value;
            }
        }
        #endregion

        //============================================================
        //	STATIC METHODS
        //============================================================
        #region MatchByType(ISyndicationExtension extension)
        /// <summary>
        /// Predicate delegate that returns a value indicating if the supplied <see cref="ISyndicationExtension"/> 
        /// represents the same <see cref="Type"/> as this <see cref="SyndicationExtension"/>.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to be compared.</param>
        /// <returns><b>true</b> if the <paramref name="extension"/> is the same <see cref="Type"/> as this <see cref="SyndicationExtension"/>; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        public static bool MatchByType(ISyndicationExtension extension)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(extension, "extension");

            //------------------------------------------------------------
            //	Determine if search condition was met 
            //------------------------------------------------------------
            if (extension.GetType() == typeof(WellFormedWebCommentsSyndicationExtension))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Load(IXPathNavigable source)
        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load this <see cref="WellFormedWebCommentsSyndicationExtension"/>.</param>
        /// <returns><b>true</b> if the <see cref="WellFormedWebCommentsSyndicationExtension"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public override bool Load(IXPathNavigable source)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded  = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Attempt to extract syndication extension information
            //------------------------------------------------------------
            XPathNavigator navigator    = source.CreateNavigator();
            wasLoaded                   = this.Context.Load(navigator, this.CreateNamespaceManager(navigator));

            //------------------------------------------------------------
            //	Raise extension loaded event
            //------------------------------------------------------------
            SyndicationExtensionLoadedEventArgs args    = new SyndicationExtensionLoadedEventArgs(source, this);
            this.OnExtensionLoaded(args);

            return wasLoaded;
        }
        #endregion

        #region Load(XmlReader reader)
        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load this <see cref="WellFormedWebCommentsSyndicationExtension"/>.</param>
        /// <returns><b>true</b> if the <see cref="WellFormedWebCommentsSyndicationExtension"/> was able to be initialized using the supplied <paramref name="reader"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        public override bool Load(XmlReader reader)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(reader, "reader");

            //------------------------------------------------------------
            //	Create navigator against reader and pass to load method
            //------------------------------------------------------------
            XPathDocument document  = new XPathDocument(reader);

            return this.Load(document.CreateNavigator());
        }
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Writes the syndication extension to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the syndication extension.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public override void WriteTo(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write current extension details to the writer
            //------------------------------------------------------------
            this.Context.WriteTo(writer, this.XmlNamespace);
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="WellFormedWebCommentsSyndicationExtension"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="WellFormedWebCommentsSyndicationExtension"/>.</returns>
        /// <remarks>
        ///     This method returns the XML representation for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            using(MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings  = new XmlWriterSettings();
                settings.ConformanceLevel   = ConformanceLevel.Fragment;
                settings.Indent             = true;
                settings.OmitXmlDeclaration = true;

                using(XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    this.WriteTo(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        #endregion

        //============================================================
        //	ICOMPARABLE IMPLEMENTATION
        //============================================================
        #region CompareTo(object obj)
        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException">The <paramref name="obj"/> is not the expected <see cref="Type"/>.</exception>
        public int CompareTo(object obj)
        {
            //------------------------------------------------------------
            //	If target is a null reference, instance is greater
            //------------------------------------------------------------
            if (obj == null)
            {
                return 1;
            }

            //------------------------------------------------------------
            //	Determine comparison result using property state of objects
            //------------------------------------------------------------
            WellFormedWebCommentsSyndicationExtension value  = obj as WellFormedWebCommentsSyndicationExtension;

            if (value != null)
            {
                int result  = Uri.Compare(this.Context.Comments, value.Context.Comments, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                result      = result | Uri.Compare(this.Context.CommentsFeed, value.Context.CommentsFeed, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);

                return result;
            }
            else
            {
                throw new ArgumentException(String.Format(null, "obj is not of type {0}, type was found to be '{1}'.", this.GetType().FullName, obj.GetType().FullName), "obj");
            }
        }
        #endregion

        #region Equals(Object obj)
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current instance.</param>
        /// <returns><b>true</b> if the specified <see cref="Object"/> is equal to the current instance; otherwise, <b>false</b>.</returns>
        public override bool Equals(Object obj)
        {
            //------------------------------------------------------------
            //	Determine equality via type then by comparision
            //------------------------------------------------------------
            if (!(obj is WellFormedWebCommentsSyndicationExtension))
            {
                return false;
            }

            return (this.CompareTo(obj) == 0);
        }
        #endregion

        #region GetHashCode()
        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            //------------------------------------------------------------
            //	Generate has code using unique value of ToString() method
            //------------------------------------------------------------
            char[] charArray    = this.ToString().ToCharArray();

            return charArray.GetHashCode();
        }
        #endregion

        #region == operator
        /// <summary>
        /// Determines if operands are equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the values of its operands are equal, otherwise; <b>false</b>.</returns>
        public static bool operator ==(WellFormedWebCommentsSyndicationExtension first, WellFormedWebCommentsSyndicationExtension second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return true;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }
        #endregion

        #region != operator
        /// <summary>
        /// Determines if operands are not equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>false</b> if its operands are equal, otherwise; <b>true</b>.</returns>
        public static bool operator !=(WellFormedWebCommentsSyndicationExtension first, WellFormedWebCommentsSyndicationExtension second)
        {
            return !(first == second);
        }
        #endregion

        #region < operator
        /// <summary>
        /// Determines if first operand is less than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is less than the second, otherwise; <b>false</b>.</returns>
        public static bool operator <(WellFormedWebCommentsSyndicationExtension first, WellFormedWebCommentsSyndicationExtension second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return true;
            }

            return (first.CompareTo(second) < 0);
        }
        #endregion

        #region > operator
        /// <summary>
        /// Determines if first operand is greater than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is greater than the second, otherwise; <b>false</b>.</returns>
        public static bool operator >(WellFormedWebCommentsSyndicationExtension first, WellFormedWebCommentsSyndicationExtension second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return (first.CompareTo(second) > 0);
        }
        #endregion
    }
}
