SELECT 
    c.name AS 'Columna',
    t.name AS 'TipoDato',
    c.max_length AS 'Longitud',
    c.is_nullable AS 'PermiteNulos',
    OBJECT_DEFINITION(c.default_object_id) AS 'ValorPredeterminado'
FROM 
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
WHERE 
    c.object_id = OBJECT_ID('Usuario')
ORDER BY 
    c.column_id; 