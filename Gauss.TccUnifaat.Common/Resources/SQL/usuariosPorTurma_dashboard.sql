SELECT 
    t.Nome as Turma, 
    COUNT(u.Id) as QuantidadeUsuariosPorTurma
FROM 
    Turmas t
    LEFT JOIN AspNetUsers u ON t.TurmaId = u.TurmaId 
WHERE 
    t.Excluido = 0
GROUP BY 
    t.Nome
ORDER BY 
    t.Nome;
