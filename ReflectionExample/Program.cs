using System;
using System.IO;
using System.Linq;
using System.Reflection;
namespace ReflectionExample
{
    public class Person
    {
        public string str;
        public int num;

        public string Name {
            get
            {
                return str;
            }
            set
            {
                str = value;
            }
        }
        public int Age { get=>num; set {  num = value; } }
        public Person()
        {
            Console.WriteLine("No Parameter Constructor");
        }
        public Person(string _str , int _num) 
        {
            str = _str;
            num = _num;
            Console.WriteLine("Have Some Parmeters Constructor");
        }
        public void Show()
        {
            Console.WriteLine("Show, name:{0},age:{1}",Name,Age);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //使用反射获取字段
            var p = new Person();
            //typeof参数 要求是类名
            Type t = typeof(Person);
            Console.WriteLine("p:"+t);

            Console.WriteLine("----------------------获取公有字段------------------------");
            //获取在类中的所有字段(公有)，我们定义的有两个str。num
            FieldInfo[] fis = t.GetFields();
            //对数组进行遍历使用foreach
            foreach (var item in fis)
            {
                Console.WriteLine(item);
            }
            //这个数组类型的静态方法foreach?
            Array.ForEach(fis,e => Console.WriteLine(e));

            //实现了IEnumeator接口 可枚举的比如list列表就可以使用下面这种写法遍历。
            fis.ToList().ForEach(e => Console.WriteLine(e));


            Console.WriteLine("-----------------------获取公有属性-----------------------");
            //获取属性(公有)
            PropertyInfo[] propertys = t.GetProperties();
            //foreach遍历
            Array.ForEach(propertys, e => Console.WriteLine(e));


            Console.WriteLine("-----------------------获取公有构造函数，并获取每个构造函数中的参数-----------------------");
            var constructors = t.GetConstructors();
            //第一个参数是数组,第二个参数是Action委托类型的方法
            Array.ForEach(constructors, (e) => { 
                Console.WriteLine(e); 
                //获取构造函数的参数
                Array.ForEach(e.GetParameters(), p => { Console.WriteLine(p); }); 
            });


            Console.WriteLine("-----------------------获取公有方法-----------------------");
            var mis = t.GetMethods();
            Array.ForEach(mis, e => Console.WriteLine(e));


            Console.WriteLine("-----------------------用获取到的信息做一些事情---------------------------");
            #region 用构造函数创建一个类的实例
            Type[] paramType = new Type[2];
            paramType[0] = typeof(string);
            paramType[1] = typeof(int);
            //根据传入的参数获取指定的构造函数
            ConstructorInfo ci = t.GetConstructor(paramType);
            //使用invoke方法，传入具体的参数
            object[] obj = new object[] { "Michael",10};
            //返回的是object类型
            var personI = ci.Invoke(obj) as Person;
            personI.Show();

            #endregion


            #region 用Activator创建类的实例
            object[] obj2 = new object[] { "Luna", 23 };

            //同样返回的是Object类型的 可以用Type作为参数
            var personI_2 = Activator.CreateInstance(t) as Person;
            personI_2.Show();

            //第二种方式 传入Type 和参数列表数组
            var personI_3 = Activator.CreateInstance(t, obj2) as Person;
            personI_3.Show();

            //还有一中 
            var personI_4 = Activator.CreateInstance(t,"Jack",22) as Person;
            personI_4.Show();

            #endregion

            Console.WriteLine("------------------------关于GeyType的用法以及通过反射对成员变量赋值-------------------------");
            Person psome = new Person();
            Type psomeType = psome.GetType();
            Console.WriteLine("tPsome:"+psomeType);

            //必须放入完全性命名 命名空间+类名
            Type t2 = Type.GetType("ReflectionExample.Person");
            Console.WriteLine("t2:"+ t2);

            //使用Activator.CreateInstance创建实例
            object objPsome = Activator.CreateInstance(psomeType);

            #region 不用强转想给对象的成员变量和成员方法赋值应该怎么办

            //通过字段名字 获取对应字段
            FieldInfo f = psomeType.GetField("str");
            //第一个参数随谁赋值 objPsome 这个对象
            f.SetValue(objPsome,"Hello");
            //获取指定对象身上字段的值
            Console.WriteLine(f.GetValue(objPsome));

            //int类型的赋值
            FieldInfo f2 = psomeType.GetField("num");
            f2.SetValue(objPsome,20);
            Console.WriteLine(f2.GetValue(objPsome));

            //property属性赋值 
            //type.GetProperty 通过名称从Type中获取对应的PropertyInfo属性
            PropertyInfo p1 = psomeType.GetProperty("Name");
            //第一个参数给谁赋值(我们创造的对象)
            p1.SetValue(objPsome, "Kate",null);
            //从对应的属性信息的getvalue方法获取对象的属性值
            Console.WriteLine(p1.GetValue(objPsome));

            PropertyInfo p2 = psomeType.GetProperty("Age");
            p2.SetValue(objPsome, 12,null);
            Console.WriteLine(p2.GetValue(objPsome));

            //调用成员方法
            MethodInfo mi = psomeType.GetMethod("Show");
            mi.Invoke(objPsome,null);

            #endregion



        }
    }
}
