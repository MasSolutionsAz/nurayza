namespace ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs
{
    public class MenuLangDto
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
    }
}