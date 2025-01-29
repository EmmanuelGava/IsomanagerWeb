-- Verificar la estructura actual de la tabla Usuario
SELECT 
    c.name AS 'ColumnName',
    t.name AS 'DataType',
    c.max_length AS 'MaxLength',
    c.is_nullable AS 'IsNullable',
    CASE WHEN dc.definition IS NOT NULL THEN dc.definition ELSE '' END AS 'DefaultValue'
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
WHERE c.object_id = OBJECT_ID('dbo.Usuario')
ORDER BY c.column_id; 