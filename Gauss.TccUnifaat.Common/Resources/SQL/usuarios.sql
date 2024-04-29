SELECT 
    u.Id as Id,
    u.NomeCompleto as NomeCompleto,
    u.Email as Email,
    u.Telefone as Telefone,
    t.Nome as Turma, 
    r.Name as Funcao
FROM 
    AspNetUsers u
    LEFT JOIN Turmas t ON u.TurmaId = t.TurmaId 
    LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
    LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE 
    u.Excluido = 0
ORDER BY 
    u.NomeCompleto, Turma, Funcao;
