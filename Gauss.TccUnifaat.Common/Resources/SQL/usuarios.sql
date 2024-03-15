SELECT 
    u.NomeCompleto as NomeCompleto,
    u.Email as Email,
    t.Nome as Turma, 
    r.Name as Funcao
FROM 
    AspNetUsers u
    LEFT JOIN Turmas t ON u.TurmaId = t.TurmaId 
    LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
    LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY 
    u.NomeCompleto, Turma, Funcao;
