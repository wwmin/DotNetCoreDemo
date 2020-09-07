using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.AutoMappers
{
    /// <summary>
    /// 此处的映射是在没有定义profile的情况下的一一映射
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        /// 单体实体类型映射, 默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TSource">原始类型</typeparam>
        /// <typeparam name="TDestionation">目标类型</typeparam>
        /// <param name="source">要被转化的数据</param>
        /// <returns>转化之后的实体</returns>
        public static TDestionation MapTo<TSource, TDestionation>(this TSource source) where TDestionation : class, new() where TSource : class
        {
            if (source == null) return new TDestionation();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestionation>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestionation>(source);
        }

        /// <summary>
        /// 实体列表类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TSource">原始类型</typeparam>
        /// <typeparam name="TDestionation">目标类型</typeparam>
        /// <param name="source">要被转化的数据</param>
        /// <returns>转化之后的实体列表</returns>
        public static List<TDestionation> MapToList<TSource, TDestionation>(this List<TSource> source)
            where TDestionation : class
            where TSource : class
        {
            if (source == null) return new List<TDestionation>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestionation>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestionation>>(source);
        }
    }
}
