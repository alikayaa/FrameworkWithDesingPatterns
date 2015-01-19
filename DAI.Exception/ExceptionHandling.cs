using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Security;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Xml.Schema;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text;

namespace DAI.Exception
{
    internal class ExceptionHandling
    {

        #region Singleton

        private static ExceptionHandling instance = null;
        private static object objectlock = new object();
        private ExceptionHandling()
        {

        }

        public static ExceptionHandling getInstance()
        {
            if (instance == null)
            {
                lock (objectlock)
                {
                    if (instance == null)
                    {
                        instance = new ExceptionHandling();
                    }
                }
            }

            return instance;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Geri dönüşü olan metodlarımızı bu generic metod sayesinde try-catch ile sarmalarız.
        /// </summary>
        /// <typeparam name="T">Yazdığımız kodda geri dönüş değeri ne ise bu generic tip de odur.</typeparam>
        /// <param name="func">Yazdığımız geri dönüş değeri olan kod satırları.</param>
        /// <param name="methodbase">Çağıran metod bilgisi</param>
        /// <param name="exceptionFormatter">Hatayı handle edecek olan formatter tipi</param>
        /// <param name="errorDesc">Varsa bizim hata açıklamamız.</param>
        /// <returns>Yazdığımız kodda geri dönüş tipi ne ise, bu metodun geri dönüş tipi de o dur.</returns>
        internal T SurroundWithTryCatch<T>(Func<T> func, MethodBase methodbase, IExceptionFormatter exceptionFormatter, string errorDesc = null, Action<System.Exception> failCallback = null, bool throwup = true)
        {
            try
            {
                return func();
            }
            catch (SEHException ex)//External Ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SEHException, exceptionFormatter);
            }
            catch (ArgumentOutOfRangeException ex)//argument ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArgumentOutOfRangeException, exceptionFormatter);
            }
            catch (ArgumentNullException ex)//argument ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArgumentNullException, exceptionFormatter);
            }
            catch (DivideByZeroException ex)//arithmetic ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DivideByZeroException, exceptionFormatter);
            }
            catch (NotFiniteNumberException ex)//arithmetic ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NotFiniteNumberException, exceptionFormatter);
            }
            catch (OverflowException ex)//arithmetic ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OverflowException, exceptionFormatter);
            }
            catch (FileNotFoundException ex)//io ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.FileNotFoundException, exceptionFormatter);
            }
            catch (PathTooLongException ex)//io ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.PathTooLongException, exceptionFormatter);
            }


            catch (ExternalException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ExternalException, exceptionFormatter);
            }
            catch (ArgumentException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArgumentException, exceptionFormatter);
            }
            catch (InvalidOperationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidOperationException, exceptionFormatter);
            }
            catch (AccessViolationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AccessViolationException, exceptionFormatter);
            }
            catch (NullReferenceException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NullReferenceException, exceptionFormatter);
            }
            catch (IndexOutOfRangeException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.IndexOutOfRangeException, exceptionFormatter);
            }
            catch (ArithmeticException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArithmeticException, exceptionFormatter);
            }
            catch (ArrayTypeMismatchException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArrayTypeMismatchException, exceptionFormatter);
            }
            catch (IOException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.IOException, exceptionFormatter);
            }
            catch (FormatException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.FormatException, exceptionFormatter);
            }
            catch (InvalidCastException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidCastException, exceptionFormatter);
            }
            catch (MulticastNotSupportedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MulticastNotSupportedException, exceptionFormatter);
            }
            catch (NotImplementedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NotImplementedException, exceptionFormatter);
            }
            catch (NotSupportedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NotSupportedException, exceptionFormatter);
            }
            catch (OutOfMemoryException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OutOfMemoryException, exceptionFormatter);
            }
            catch (SecurityException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SecurityException, exceptionFormatter);
            }
            catch (InvalidUdtException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidUdtException, exceptionFormatter);
            }
            catch (AppDomainUnloadedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AppDomainUnloadedException, exceptionFormatter);
            }
            catch (BadImageFormatException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.BadImageFormatException, exceptionFormatter);
            }
            catch (CannotUnloadAppDomainException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.CannotUnloadAppDomainException, exceptionFormatter);
            }
            catch (KeyNotFoundException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.KeyNotFoundException, exceptionFormatter);
            }
            catch (LicenseException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.LicenseException, exceptionFormatter);
            }
            catch (WarningException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.WarningException, exceptionFormatter);
            }
            catch (ConfigurationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ConfigurationException, exceptionFormatter);
            }
            catch (ContextMarshalException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ContextMarshalException, exceptionFormatter);
            }
            catch (DataException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DataException, exceptionFormatter);
            }
            catch (DBConcurrencyException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DBConcurrencyException, exceptionFormatter);
            }
            catch (OperationAbortedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OperationAbortedException, exceptionFormatter);
            }
            catch (SqlTypeException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SqlTypeException, exceptionFormatter);
            }
            catch (DataMisalignedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DataMisalignedException, exceptionFormatter);
            }
            catch (ExecutionEngineException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ExecutionEngineException, exceptionFormatter);
            }
            catch (InsufficientExecutionStackException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InsufficientExecutionStackException, exceptionFormatter);
            }
            catch (InvalidProgramException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidProgramException, exceptionFormatter);
            }
            catch (InternalBufferOverflowException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InternalBufferOverflowException, exceptionFormatter);
            }
            catch (InvalidDataException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidDataException, exceptionFormatter);
            }
            catch (MemberAccessException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MemberAccessException, exceptionFormatter);
            }
            catch (OperationCanceledException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OperationCanceledException, exceptionFormatter);
            }
            catch (RankException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.RankException, exceptionFormatter);
            }
            catch (AmbiguousMatchException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AmbiguousMatchException, exceptionFormatter);
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ReflectionTypeLoadException, exceptionFormatter);
            }
            catch (MissingManifestResourceException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MissingManifestResourceException, exceptionFormatter);
            }
            catch (MissingSatelliteAssemblyException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MissingSatelliteAssemblyException, exceptionFormatter);
            }
            catch (SerializationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SerializationException, exceptionFormatter);
            }
            catch (AuthenticationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AuthenticationException, exceptionFormatter);
            }
            catch (CryptographicException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.HostProtectionException, exceptionFormatter);
            }
            catch (HostProtectionException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.HostProtectionException, exceptionFormatter);
            }
            catch (PolicyException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.PolicyException, exceptionFormatter);
            }
            catch (IdentityNotMappedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.IdentityNotMappedException, exceptionFormatter);
            }
            catch (VerificationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.VerificationException, exceptionFormatter);
            }
            catch (XmlSyntaxException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XmlSyntaxException, exceptionFormatter);
            }
            catch (AbandonedMutexException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AbandonedMutexException, exceptionFormatter);
            }
            catch (SemaphoreFullException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SemaphoreFullException, exceptionFormatter);
            }
            catch (SynchronizationLockException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SynchronizationLockException, exceptionFormatter);
            }
            catch (ThreadAbortException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadAbortException, exceptionFormatter);
            }
            catch (ThreadInterruptedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadInterruptedException, exceptionFormatter);
            }
            catch (ThreadStartException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadStartException, exceptionFormatter);
            }
            catch (ThreadStateException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadStateException, exceptionFormatter);
            }
            catch (TimeoutException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TimeoutException, exceptionFormatter);
            }
            catch (TypeInitializationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TypeInitializationException, exceptionFormatter);
            }
            catch (TypeLoadException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TypeLoadException, exceptionFormatter);
            }
            catch (TypeUnloadedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TypeUnloadedException, exceptionFormatter);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.UnauthorizedAccessException, exceptionFormatter);
            }
            catch (XmlSchemaException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XmlSchemaException, exceptionFormatter);
            }
            catch (XmlException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XmlException, exceptionFormatter);
            }
            catch (XPathException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XPathException, exceptionFormatter);
            }
            catch (XsltException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XsltException, exceptionFormatter);
            }
            catch (SystemException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SystemException, exceptionFormatter);
            }
            catch (System.Exception ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowErrorMini(methodbase, errorDesc, ex, ExceptionTypes.Exception, exceptionFormatter);
            }

            object o = Activator.CreateInstance(typeof(T));
            return (T)o;
        }

        /// <summary>
        /// Geri dönüşü olmayan metodlarımızı bu metod sayesinde try-catch ile sarmalarız.
        /// </summary>
        /// <param name="method">Geri dönüşü olmayan yazacağınız her türlü kod.</param>
        /// <param name="methodbase">Çağıran metod bilgisi</param>
        /// <param name="exceptionFormatter">Exception formatlayıcı</param>
        /// <param name="errorDesc">Varsa bizim hata açıklamamız. Olmak zorunda değil</param>
        internal void SurroundWithTryCatch(Action method, MethodBase methodbase, IExceptionFormatter exceptionFormatter, string errorDesc = null,Action<System.Exception> failCallback=null,bool throwup=true)
        {
            try
            {
                method();
            }
            catch (SEHException ex)//External Ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SEHException, exceptionFormatter);
            }
            catch (ArgumentOutOfRangeException ex)//argument ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArgumentOutOfRangeException, exceptionFormatter);
            }
            catch (ArgumentNullException ex)//argument ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArgumentNullException, exceptionFormatter);
            }
            catch (DivideByZeroException ex)//arithmetic ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DivideByZeroException, exceptionFormatter);
            }
            catch (NotFiniteNumberException ex)//arithmetic ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NotFiniteNumberException, exceptionFormatter);
            }
            catch (OverflowException ex)//arithmetic ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OverflowException, exceptionFormatter);
            }
            catch (FileNotFoundException ex)//io ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.FileNotFoundException, exceptionFormatter);
            }
            catch (PathTooLongException ex)//io ex altında
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.PathTooLongException, exceptionFormatter);
            }


            catch (ExternalException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ExternalException, exceptionFormatter);
            }
            catch (ArgumentException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArgumentException, exceptionFormatter);
            }
            catch (InvalidOperationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidOperationException, exceptionFormatter);
            }
            catch (AccessViolationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AccessViolationException, exceptionFormatter);
            }
            catch (NullReferenceException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NullReferenceException, exceptionFormatter);
            }
            catch (IndexOutOfRangeException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.IndexOutOfRangeException, exceptionFormatter);
            }
            catch (ArithmeticException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArithmeticException, exceptionFormatter);
            }
            catch (ArrayTypeMismatchException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ArrayTypeMismatchException, exceptionFormatter);
            }
            catch (IOException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.IOException, exceptionFormatter);
            }
            catch (FormatException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.FormatException, exceptionFormatter);
            }
            catch (InvalidCastException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidCastException, exceptionFormatter);
            }
            catch (MulticastNotSupportedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MulticastNotSupportedException, exceptionFormatter);
            }
            catch (NotImplementedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NotImplementedException, exceptionFormatter);
            }
            catch (NotSupportedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.NotSupportedException, exceptionFormatter);
            }
            catch (OutOfMemoryException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OutOfMemoryException, exceptionFormatter);
            }
            catch (SecurityException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SecurityException, exceptionFormatter);
            }
            catch (InvalidUdtException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidUdtException, exceptionFormatter);
            }
            catch (AppDomainUnloadedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AppDomainUnloadedException, exceptionFormatter);
            }
            catch (BadImageFormatException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.BadImageFormatException, exceptionFormatter);
            }
            catch (CannotUnloadAppDomainException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.CannotUnloadAppDomainException, exceptionFormatter);
            }
            catch (KeyNotFoundException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.KeyNotFoundException, exceptionFormatter);
            }
            catch (LicenseException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.LicenseException, exceptionFormatter);
            }
            catch (WarningException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.WarningException, exceptionFormatter);
            }
            catch (ConfigurationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ConfigurationException, exceptionFormatter);
            }
            catch (ContextMarshalException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ContextMarshalException, exceptionFormatter);
            }
            catch (DataException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DataException, exceptionFormatter);
            }
            catch (DBConcurrencyException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DBConcurrencyException, exceptionFormatter);
            }
            catch (OperationAbortedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OperationAbortedException, exceptionFormatter);
            }
            catch (SqlTypeException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SqlTypeException, exceptionFormatter);
            }
            catch (DataMisalignedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.DataMisalignedException, exceptionFormatter);
            }
            catch (ExecutionEngineException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ExecutionEngineException, exceptionFormatter);
            }
            catch (InsufficientExecutionStackException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InsufficientExecutionStackException, exceptionFormatter);
            }
            catch (InvalidProgramException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidProgramException, exceptionFormatter);
            }
            catch (InternalBufferOverflowException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InternalBufferOverflowException, exceptionFormatter);
            }
            catch (InvalidDataException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.InvalidDataException, exceptionFormatter);
            }
            catch (MemberAccessException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MemberAccessException, exceptionFormatter);
            }
            catch (OperationCanceledException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.OperationCanceledException, exceptionFormatter);
            }
            catch (RankException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.RankException, exceptionFormatter);
            }
            catch (AmbiguousMatchException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AmbiguousMatchException, exceptionFormatter);
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ReflectionTypeLoadException, exceptionFormatter);
            }
            catch (MissingManifestResourceException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MissingManifestResourceException, exceptionFormatter);
            }
            catch (MissingSatelliteAssemblyException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.MissingSatelliteAssemblyException, exceptionFormatter);
            }
            catch (SerializationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SerializationException, exceptionFormatter);
            }
            catch (AuthenticationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AuthenticationException, exceptionFormatter);
            }
            catch (CryptographicException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.HostProtectionException, exceptionFormatter);
            }
            catch (HostProtectionException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.HostProtectionException, exceptionFormatter);
            }
            catch (PolicyException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.PolicyException, exceptionFormatter);
            }
            catch (IdentityNotMappedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.IdentityNotMappedException, exceptionFormatter);
            }
            catch (VerificationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.VerificationException, exceptionFormatter);
            }
            catch (XmlSyntaxException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XmlSyntaxException, exceptionFormatter);
            }
            catch (AbandonedMutexException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.AbandonedMutexException, exceptionFormatter);
            }
            catch (SemaphoreFullException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SemaphoreFullException, exceptionFormatter);
            }
            catch (SynchronizationLockException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SynchronizationLockException, exceptionFormatter);
            }
            catch (ThreadAbortException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadAbortException, exceptionFormatter);
            }
            catch (ThreadInterruptedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadInterruptedException, exceptionFormatter);
            }
            catch (ThreadStartException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadStartException, exceptionFormatter);
            }
            catch (ThreadStateException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.ThreadStateException, exceptionFormatter);
            }
            catch (TimeoutException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TimeoutException, exceptionFormatter);
            }
            catch (TypeInitializationException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TypeInitializationException, exceptionFormatter);
            }
            catch (TypeLoadException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TypeLoadException, exceptionFormatter);
            }
            catch (TypeUnloadedException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.TypeUnloadedException, exceptionFormatter);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.UnauthorizedAccessException, exceptionFormatter);
            }
            catch (XmlSchemaException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XmlSchemaException, exceptionFormatter);
            }
            catch (XmlException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XmlException, exceptionFormatter);
            }
            catch (XPathException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XPathException, exceptionFormatter);
            }
            catch (XsltException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.XsltException, exceptionFormatter);
            }


            catch (SystemException ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowError(methodbase, errorDesc, ex, ExceptionTypes.SystemException, exceptionFormatter);
            }


            catch (System.Exception ex)
            {
                if (failCallback != null) failCallback(ex);
                if (throwup) throw ThrowErrorMini(methodbase, errorDesc, ex, ExceptionTypes.Exception, exceptionFormatter);
            }
        }


        #endregion

        #region Private methods

        /// <summary>
        /// Kodun gerçekten asıl patladığı yerde bu metod çağırılır. Formatlanır ve daha sonra
        /// yukarıya fırlatılır.
        /// </summary>
        /// <param name="methodbase">Çağıran metod bilgisi</param>
        /// <param name="errorDesc">Varsa bizim hata açıklamamız</param>
        /// <param name="ex">Exception nesnesi</param>
        /// <param name="exType">Exception tipi</param>
        /// <param name="exceptionFormatter">Exceptionı handle edecek formatter.</param>
        /// <returns>Geriye içerisinde formatlanan hata olan hatayı Exception tipinde geriye döner</returns>
        private System.Exception ThrowError(MethodBase methodbase, string errorDesc, System.Exception ex, ExceptionTypes exType, IExceptionFormatter exceptionFormatter)
        {
            string error = exceptionFormatter.FormatException(methodbase, errorDesc, ex, exType);
            ex.HelpLink = exType.ToString();
            return new System.Exception(error, ex);
        }

        /// <summary>
        /// Hatanın geçtiği yolda yukarıya fırlatılan hatalar, exception catch' ine düşerler
        /// Exception catch' in içerisinde de bu metod çağırılır. Formatlanır ve daha sonra
        /// yukarıya fırlatılır.
        /// </summary>
        /// <param name="methodbase">Çağıran metod bilgisi</param>
        /// <param name="errorDesc">Varsa bizim hata açıklamamız</param>
        /// <param name="ex">Exception nesnesi</param>
        /// <param name="exType">Exception tipi</param>
        /// <param name="exceptionFormatter">Exceptionı handle edecek formatter.</param>
        /// <returns>Geriye içerisinde formatlanan hata olan hatayı Exception tipinde geriye döner</returns>
        private System.Exception ThrowErrorMini(MethodBase methodbase, string errorDesc, System.Exception ex, ExceptionTypes exType, IExceptionFormatter exceptionFormatter)
        {
            string error = exceptionFormatter.FormatExceptionThrowCatch(methodbase, errorDesc, ex, exType);
            return new System.Exception(error, ex);
        }

        #endregion

    }
}
