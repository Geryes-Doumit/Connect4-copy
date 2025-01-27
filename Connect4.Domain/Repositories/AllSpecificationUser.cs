using Connect4.Common.Contracts;
using Connect4.Common.Model;
using System;
using System.Linq.Expressions;

namespace Connect4.Domain.Repositories;

public class AllSpecificationUser : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression() => t => true;
}