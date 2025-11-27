using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
	public partial class MatriculaListViewModel : BaseViewModel
	{
		public ObservableCollection<string> FilterTypes { get; } = new() { "Id", "Aluno", "Ativas"};
		private readonly IMatriculaService _matriculaService;
		private string _searchText = string.Empty;
		public string SearchText
		{
			get => _searchText;
			set => SetProperty(ref _searchText, value);
		}
		private string _selectedFilterType = "Ativas";
		public string SelectedFilterType
		{
			get => _selectedFilterType;
			set => SetProperty(ref _selectedFilterType, value);
		}
		private ObservableCollection<MatriculaDTO> _Matriculas = new();
		public ObservableCollection<MatriculaDTO> Matriculas
		{
			get => _Matriculas;
			set => SetProperty(ref _Matriculas, value);
		}
		private MatriculaDTO? _selectedMatricula;
		public MatriculaDTO? SelectedMatricula
		{
			get => _selectedMatricula;
			set => SetProperty(ref _selectedMatricula, value);
		}
		public MatriculaListViewModel(IMatriculaService MatriculaService)
		{
			_matriculaService = MatriculaService;
			Title = "Matriculas";
		}
		// métodos de comando

		[RelayCommand]
		private async Task AddMatriculaAsync()
		{
			try
			{
				await Shell.Current.GoToAsync("matricula");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Erro", $"Erro ao navegar para tela de cadastro: {ex.Message}", "OK");
			}
		}
		[RelayCommand]
		private async Task EditMatriculaAsync(MatriculaDTO Matricula)
		{
			try
			{
				if (Matricula == null)
					return;
				await Shell.Current.GoToAsync($"matricula?Id={Matricula.Id}");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Erro", $"Erro ao navegar para tela de edição: {ex.Message}", "OK");
			}
		}
		[RelayCommand]
		private async Task RefreshAsync()
		{
			IsRefreshing = true;
			await LoadMatriculasAsync();
		}

		[RelayCommand]
		private async Task SearchMatriculasAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				// Limpa a lista atual

				await MainThread.InvokeOnMainThreadAsync(() =>

				{
					Matriculas.Clear();
				});
				IEnumerable<MatriculaDTO> resultados = Enumerable.Empty<MatriculaDTO>();
				// Busca os Matriculas de acordo com o filtro
				if (string.IsNullOrWhiteSpace(SearchText))

				{
					resultados = await _matriculaService.ObterTodasAsync() ?? Enumerable.Empty<MatriculaDTO>();
				}
				else if (SelectedFilterType == "Id" && int.TryParse(SearchText, out int id))
				{
					var Matricula = await _matriculaService.ObterPorIdAsync(id);

					if (Matricula != null)
						resultados = new[] { Matricula };
				}
				else if (SelectedFilterType == "Ativas")
				{
					var Matriculas = await _matriculaService.ObterAtivasAsync();

					if (Matriculas != null)
						resultados = Matriculas;
				}
				else if (SelectedFilterType == "Aluno" && int.TryParse(SearchText, out int idAluno))
				{
					var Matriculas = await _matriculaService.ObterPorAlunoIdAsync(idAluno);

					if (Matriculas != null)
						resultados = Matriculas;
				}
				// Atualiza a coleção na thread principal

				await MainThread.InvokeOnMainThreadAsync(() =>

				{
					foreach (var item in resultados)
					{
						Matriculas.Add(item);
					}
					OnPropertyChanged(nameof(Matriculas));
				});
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar Matriculas: {ex.Message}", "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		[RelayCommand]
		private async Task LoadMatriculasAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				// Limpa a lista atual antes de carregar novos dados
				await MainThread.InvokeOnMainThreadAsync(() =>

				{
					Matriculas.Clear();
					OnPropertyChanged(nameof(Matriculas));
				});
				var MatriculasList = await _matriculaService.ObterTodasAsync();
				if (MatriculasList != null)
				{
					// Garantir que a atualização da UI aconteça na thread principal

					await MainThread.InvokeOnMainThreadAsync(() =>

					{
						foreach (var Matricula in MatriculasList)
						{
							Matriculas.Add(Matricula);
						}
						OnPropertyChanged(nameof(Matriculas));
					});
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar Matriculas: {ex.Message}", "OK");
			}
			finally
			{
				IsBusy = false;
				IsRefreshing = false;
			}
		}

		[RelayCommand]
		private async Task DeleteMatriculaAsync(MatriculaDTO Matricula)
		{
			if (Matricula == null)
				return;
			bool confirm = await Shell.Current.DisplayAlert(
			"Confirmar Exclusão",

			$"Deseja realmente excluir a Matricula {Matricula.Id}?",
			"Sim", "Não");
			if (!confirm)
				return;
			try
			{
				IsBusy = true;
				bool success = await _matriculaService.RemoverAsync(Matricula.Id);
				if (success)
				{
					Matriculas.Remove(Matricula);
					await Shell.Current.DisplayAlert("Sucesso", "Matricula excluído com sucesso!", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Erro", "Não foi possível excluir o Matricula.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Erro", $"Erro ao excluir Matricula: {ex.Message}", "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
