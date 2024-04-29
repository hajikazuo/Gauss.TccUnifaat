SELECT 
    t.Nome as Turma, 
    COUNT(u.Id) as QuantidadeUsuariosPorTurma
FROM 
    Turmas t
    LEFT JOIN AspNetUsers u ON t.TurmaId = u.TurmaId
WHERE 
    u.Excluido = 0 OR u.Excluido IS NULL
GROUP BY 
    t.Nome
ORDER BY 
    t.Nome;
