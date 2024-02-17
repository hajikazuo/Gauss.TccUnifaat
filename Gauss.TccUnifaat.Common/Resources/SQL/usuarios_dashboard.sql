SELECT r.Name as Funcao, COUNT(ur.UserId) as QuantidadeUsuarios
    FROM AspNetRoles r
    LEFT JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
    GROUP BY r.Name
    ORDER BY QuantidadeUsuarios DESC;