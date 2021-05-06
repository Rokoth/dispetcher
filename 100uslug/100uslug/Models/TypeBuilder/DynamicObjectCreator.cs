using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace _100uslug.Models
{
    public static class DynamicTypeBuilder
    {
        private static ModuleBuilder _moduleBuilder;

        public static Dictionary<string, Type> DynamicTypes { get; } = new Dictionary<string, Type>();

        private static void CreateModuleBuilder()
        {
            var an = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        }

        public static object CreateNewObject(string typeSignature, List<Field> fields)
        {
            var newType = CreateNewType(typeSignature, fields);
            var newObject = Activator.CreateInstance(newType);
            return newObject;
        }
        public static Type CreateNewType(string typeSignature, List<Field> fields)
        {
            TypeBuilder tb = GetTypeBuilder(typeSignature);
            tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            foreach (var field in fields)
                CreateProperty(tb, field.FieldName, field.FieldType);
            Type objectType = tb.CreateType();
            DynamicTypes.Add(typeSignature, objectType);
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string typeSignature)
        {
            if (_moduleBuilder == null) CreateModuleBuilder();
            TypeBuilder tb = _moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }

    public class Field
    {
        public string FieldName;
        public Type FieldType;
    }
}