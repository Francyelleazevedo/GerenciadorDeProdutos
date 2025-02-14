using GerenciadorDeProdutos.Web.Models;
using GerenciadorDeProdutos.Application.Interfaces;
using GerenciadorDeProdutos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GerenciadorDeProdutos.Web.Controllers
{
    public class ProdutoController(IProdutoService produtoService) : Controller
    {
        private readonly IProdutoService _produtoService = produtoService;

        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;
                var pagedResult = await _produtoService.GetAllAsync(pageNumber, pageSize);
                return View(pagedResult);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Ocorreu um erro ao carregar os produtos.", Exception = ex });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var produto = await _produtoService.GetByIdAsync(id);
                if (produto == null) return NotFound();
                return View(produto);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Ocorreu um erro ao carregar os detalhes do produto.", Exception = ex });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (!ModelState.IsValid) return View(produto);

            try
            {
                await _produtoService.AddAsync(produto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Ocorreu um erro ao criar o produto.", Exception = ex });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var produto = await _produtoService.GetByIdAsync(id);
                if (produto == null) return NotFound();
                return View(produto);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Ocorreu um erro ao carregar o produto para edição.", Exception = ex });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            if (id != produto.Id) return NotFound();
            if (!ModelState.IsValid) return View(produto);

            try
            {
                await _produtoService.UpdateAsync(produto);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProdutoExists(produto.Id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Ocorreu um erro ao atualizar o produto.", Exception = ex });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var produto = await _produtoService.GetByIdAsync(id);
                if (produto == null) return NotFound();
                await _produtoService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Ocorreu um erro ao excluir o produto.", Exception = ex });
            }
        }

        private async Task<bool> ProdutoExists(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            return produto != null;
        }
    }
}
