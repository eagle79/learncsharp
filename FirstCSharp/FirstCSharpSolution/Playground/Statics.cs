using System;
namespace Playground
{
    public class Statics
    {
        public static string StaticProperty { get; set; }

        static Statics()
        {
            Console.WriteLine("static Statics()");
        }

        public static string GetClassJSON()
        {
            string json = $"Statics<static>: {{ StaticProperty: '{StaticProperty}' }}";
            return json;
        }

        public Statics()
        {
            Console.WriteLine("public Statics()");
        }

        public string NonStaticProperty { get; set; }

        public string GetInstanceJSON()
        {
            string json = $"Statics<instance>: {{ NonStaticPropery : '{NonStaticProperty}' }}";
            return json;
        }
    }
}
