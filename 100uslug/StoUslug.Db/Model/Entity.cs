//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 1
using StoUslug.Db.Attributes;
using System;

namespace StoUslug.Db.Model
{
    /// <summary>
    /// Общий класс-предок для всех моделей БД
    /// </summary>
    public abstract class Entity: IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [PrimaryKey]
        [ColumnName("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        [ColumnName("version_date")]
        public DateTimeOffset VersionDate { get; set; }
        /// <summary>
        /// Флаг удаления
        /// </summary>
        [ColumnName("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}