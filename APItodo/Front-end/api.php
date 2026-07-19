<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit;
}

define('API_BASE', 'http://localhost:5263/api/Tareas');

function callApi(string $method, string $path, ?array $body = null): void
{
    $url = API_BASE . $path;
    $ch = curl_init($url);

    $headers = ['Content-Type: application/json'];

    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_CUSTOMREQUEST  => $method,
        CURLOPT_HTTPHEADER     => $headers,
        CURLOPT_TIMEOUT        => 10,
    ]);

    if ($body !== null) {
        curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($body));
    }

    $respuesta = curl_exec($ch);
    $codigo    = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    $error     = curl_error($ch);
    curl_close($ch);

    if ($error) {
        http_response_code(500);
        echo json_encode(['error' => 'Error de conexión: ' . $error]);
        return;
    }

    http_response_code($codigo);
    echo $respuesta;
}

$metodo = $_SERVER['REQUEST_METHOD'];
$id     = isset($_GET['id']) ? (int)$_GET['id'] : null;

switch ($metodo) {
    case 'GET':
        $path = $id !== null ? "/$id" : '';
        callApi('GET', $path);
        break;

    case 'POST':
        $body = json_decode(file_get_contents('php://input'), true);
        callApi('POST', '', $body);
        break;

    case 'PUT':
        if ($id === null) {
            http_response_code(400);
            echo json_encode(['error' => 'Se requiere el parámetro id']);
            exit;
        }
        $body = json_decode(file_get_contents('php://input'), true);
        callApi('PUT', "/$id", $body);
        break;

    case 'DELETE':
        if ($id === null) {
            http_response_code(400);
            echo json_encode(['error' => 'Se requiere el parámetro id']);
            exit;
        }
        callApi('DELETE', "/$id");
        break;

    default:
        http_response_code(405);
        echo json_encode(['error' => 'Método no permitido']);
}
